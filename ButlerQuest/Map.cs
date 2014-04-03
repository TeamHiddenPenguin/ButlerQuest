using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace ButlerQuest
{
    public class Map
    {
        //A list that uses the gid of the tile as an index to reference the tile's texture map.
        List<Texture2D> gidToTileSet;
        //A list that uses the gid of the tile as an index to reference the tile's texture source rectangle.
        List<Rectangle> gidToRect;
        //A list of all tile layers. Tile layers are represented as 2D arrays of global IDs (gid)
        List<int[,]> layerGidMaps;
        //A dictionary that converts the name of a given object group to it's associated object group. The nested List contains each TmxObject associated with the key provided for the ObjectGroup
        public Dictionary<string, List<TiledObject>> ObjectGroups;

        //Map info
        //The width of the map
        public int Width { get; private set; }

        //The height of the map
        public int Height { get; private set; }

        //The default tile width of the map
        public int TileWidth { get; private set; }

        //The default tile height of the map
        public int TileHeight { get; private set; }

        SpriteBatch spriteBatch; //spritebatch reference
        GraphicsDevice graphicsDevice; //graphicsdevice reference

        //The graph object for this map
        public SquareGraph Graph { get; private set; }

        public Map(string filepath)
        {
            spriteBatch = ScreenManager.SharedManager.sBatch;
            graphicsDevice = ScreenManager.SharedManager.gDevice;

            //Create all of those data structures from before.
            gidToTileSet = new List<Texture2D>();
            gidToRect = new List<Rectangle>();
            layerGidMaps = new List<int[,]>();
            ObjectGroups = new Dictionary<string, List<TiledObject>>();

            //The map file (as a .tmx), interpreted as an XML file.
            XDocument xDoc = XDocument.Load("Content\\" + filepath);

            //The root node of the XML document for the map.
            XElement xMap = xDoc.Element("map");

            //Parse map info
            ParseInfo(xMap);

            //Make the tilesets
            MakeTileset(xMap);

            //Fill the 2D arrays of tile maps
            MakeTilemap(xMap);

            //Fill the Object Groups
            MakeObjects(xMap);

            //constrcut a "graph". I put graph in quotes because it's a pretty bad implementation
            Graph = CreateSquareGraph(new int[3]{1,0,0});
        }

        /// <summary>
        /// Parses basic map information
        /// </summary>
        /// <param name="xMap">the map to get info from</param>
        private void ParseInfo(XElement xMap)
        {
            //Get all of the map info properties from the "map" node
            Width = (int)xMap.Attribute("width");
            Height = (int)xMap.Attribute("height");
            TileWidth = (int)xMap.Attribute("tilewidth");
            TileHeight = (int)xMap.Attribute("tileheight");
        }

        /// <summary>
        /// Makes the tilesets from the map
        /// </summary>
        /// <param name="xMap">the map element to get the tileset data from</param>
        private void MakeTileset(XElement xMap)
        {
            //Prepare the tileset data
            foreach (XElement xTileset in xMap.Elements("tileset"))
            {
                //Start by getting a texture2D from the image's source location specified in the tiles
                string filepath = (string)(xTileset.Element("image").Attribute("source"));
                Texture2D tilemap = Texture2D.FromStream(graphicsDevice, File.OpenRead("Content\\" + filepath));

                //get the first gid of the tilemap layer
                int currentGid = (int)(xTileset.Attribute("firstgid")) - 1;

                //get tile information from the tileset
                int tilewidth = (int)(xTileset.Attribute("tilewidth"));
                int tileheight = (int)(xTileset.Attribute("tileheight"));

                //get the number of tiles in width and heigh
                int width = tilemap.Width / tilewidth;
                int height = tilemap.Height / tileheight;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //precompute tile rectangles
                        Rectangle tileRect = new Rectangle(x * tilewidth, y * tileheight, tilewidth, tileheight);
                        if (currentGid < gidToRect.Count)
                        {
                            gidToRect[currentGid] = tileRect;
                            gidToTileSet[currentGid] = tilemap;
                        }
                        else
                        {
                            gidToRect.Add(tileRect);
                            gidToTileSet.Add(tilemap);
                        }
                        currentGid++;
                    }
                }
            }
        }

        /// <summary>
        /// Parses the tile gid data from the TMX file
        /// </summary>
        /// <param name="xMap">the map element to get the tilemap data from</param>
        public void MakeTilemap(XElement xMap)
        {
            //Prepare the layer data
            foreach (XElement xLayer in xMap.Elements("layer"))
            {
                //create a 2d array of integers, representing this layer's gid map
                int[,] idMap = new int[Height, Width];

                //Get the base64-encoded string and decode it back to bytes
                string rawData = (string)xLayer.Element("data").Value;
                byte[] decodedData = Convert.FromBase64String(rawData);

                //Convert the bytes into a stream and read then in with a BinaryReader
                using (BinaryReader reader = new BinaryReader(new MemoryStream(decodedData, false)))
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            idMap[y, x] = reader.ReadInt32() - 1;
                        }
                    }
                }

                //Add the layer to the list of layers
                layerGidMaps.Add(idMap);
            }
        }

        /// <summary>
        /// Parses object information from the TMX document
        /// </summary>
        /// <param name="xMap">the map element to get the object data from</param>
        public void MakeObjects(XElement xMap)
        {
            foreach (XElement objectgroup in xMap.Elements("objectgroup"))
            {
                string groupname = (string)objectgroup.Attribute("name");
                List<TiledObject> objects = new List<TiledObject>();

                foreach (XElement obj in objectgroup.Elements("object"))
                {
                    objects.Add(new TiledObject(obj));
                }

                ObjectGroups.Add(groupname, objects);
            }
        }

        /// <summary>
        /// Draws the currently visible segment of the map to a Texture2D
        /// </summary>
        /// <param name="visibleArea">The part of the map that we can currently see</param>
        /// <param name="layerNumber">The layer to draw to a texture</param>
        /// <returns>A Texture2D showing the visible area of the map at a given layer</returns>
        public Texture2D DrawToTexture(Rectangle visibleArea, int layerNumber)
        {
            // Initialize the renderTarget
            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.CornflowerBlue);

            // Draw tiles
            Matrix cameraMatrix = Matrix.CreateTranslation(-(int)visibleArea.X, -(int)visibleArea.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cameraMatrix);

            Draw(visibleArea, layerNumber);

            spriteBatch.End();

            // Close render target
            graphicsDevice.SetRenderTarget(null);
            return renderTarget;

        }

        /// <summary>
        /// Draws the map
        /// </summary>
        /// <param name="visibleArea">The part of the map that we can currently see</param>
        /// <param name="layerNumber">The layer to draw to a texture</param>
        public void Draw(Rectangle visibleArea, int layerNumber)
        {
            int[,] idMap = layerGidMaps[layerNumber];
            for (int y = visibleArea.Y / (int)(TileHeight); y < ((visibleArea.Y + visibleArea.Height) / TileHeight) + 1; y++)
            {
                for (int x = visibleArea.X / (int)(TileWidth); x < ((visibleArea.X + visibleArea.Width) / TileWidth) + 1; x++)
                {
                    if (y > -1 && x > -1 && y < Height && x < Width)
                    {
                        int id = idMap[y, x];

                        // if the cell is unmapped, don't draw it
                        if (id > -1)
                        {
                            //Calculate position of the tile
                            Vector2 position = new Vector2(TileWidth * x, TileHeight * y);

                            //Draw it
                            spriteBatch.Draw(gidToTileSet[id], position, gidToRect[id], Color.White, 0.0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a SquareGraph of the tilemap
        /// </summary>
        /// <param name="gidToCost">A list representing the cost to traverse each gid in the tilemap</param>
        /// <returns>A SquareGraph representing the tilemap</returns>
        public SquareGraph CreateSquareGraph(List<int> gidToCost)
        {
            List<SquareGraphNode> nodes = new List<SquareGraphNode>();
            for (int z = 0; z < layerGidMaps.Count; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        SquareGraphNode node = new SquareGraphNode(x, y, z, gidToCost[layerGidMaps[z][y, x]]);
                        if(node.Cost == 0)//Only node with 0 cost are the z-traversals
                        {
                            //last gid available is down->up, last-1 is up->down
                            if(layerGidMaps[z][y, x] == gidToCost.Count - 1)
                            {
                                node.HasConnectionUpwards = true;
                            }
                            else
                            {
                                node.HasConnectionDownwards = true;
                            }
                        }
                        nodes.Add(node);
                    }
                }
            }
            return new SquareGraph(Width, Height, TileWidth, TileHeight, nodes);
        }

        /// <summary>
        /// Creates a SquareGraph of the tilemap
        /// </summary>
        /// <param name="gidToCost">an array representing the cost to traverse each gid in the tilemap</param>
        /// <returns>A SquareGraph representing the tilemap</returns>
        public SquareGraph CreateSquareGraph(int[] gidToCost)
        {
            List<SquareGraphNode> nodes = new List<SquareGraphNode>();
            for (int z = 0; z < layerGidMaps.Count; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var Gid = layerGidMaps[z][y, x];
                        if(Gid < 0) continue;
                        SquareGraphNode node = new SquareGraphNode(x, y, z, gidToCost[Gid]);
                        if (node.Cost == 0)//Only node with 0 cost are the z-traversals
                        {
                            //last gid available is down->up, last-1 is up->down
                            if (layerGidMaps[z][y, x] == gidToCost.Length - 1)
                            {
                                node.HasConnectionUpwards = true;
                            }
                            else
                            {
                                node.HasConnectionDownwards = true;
                            }
                        }
                        nodes.Add(node);
                    }
                }
            }
            return new SquareGraph(Width, Height, TileWidth, TileHeight, nodes);
        }
    }
    public class TiledObject
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public List<Tuple<string, string>> Properties { get; private set; }

        public TiledObject(XElement xObject)
        {
            //Name of the TiledObject
            string nameStr = (string)xObject.Attribute("name");
            Name = (nameStr != null) ? nameStr : "";

            //"Type" of the TiledObject
            string typeStr = (string)xObject.Attribute("type");
            Type = (typeStr != null) ? typeStr : "";

            //The X and Y position of the TiledObject.
            //These are always present, so they do not require null checks.
            X = (int)xObject.Attribute("x");
            Y = (int)xObject.Attribute("y");

            //int? = nullable integer. I don't know exactly how this works, but according to a lot of google searches this is the easiest way to handle possible null integers

            //The Width and Height of the TiledObject
            int? width = (int?)xObject.Attribute("width");
            Width = (width != null) ? (int)width : 0;

            int? height = (int?)xObject.Attribute("height");
            Height = (height != null) ? (int)height : 0;

            Properties = new List<Tuple<string, string>>();
            XElement properties = xObject.Element("properties");
            if (properties != null)
            {
                foreach (XElement property in xObject.Element("properties").Elements("property"))
                {
                    Properties.Add(new Tuple<string, string>((string)property.Attribute("name"), (string)property.Attribute("value")));
                }
            }
        }
    }
}
