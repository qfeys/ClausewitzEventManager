using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClausewitzEventManager
{
    class Parser
    {
        static List<Item> modData;

        static public void ParseAllFiles()
        {
            string path = @"Mods\";
            List<string> mods = Directory.EnumerateDirectories(path).ToList();
            List<string> allPaths = mods.Select(m => 
                                                    Directory.GetFiles(m + @"\Common\", "*.txt", SearchOption.AllDirectories)
                                                ).SelectMany(p => p.ToList()).ToList();
            try
            {
                // parse all the paths
                modData = allPaths.ConvertAll(p => Parse(p));
            }
            catch (Item.BadModException e)
            {
                Debug.Log(e.ToString());
            }
        }

        static public void ParseCommon(string path)
        {
            var files = Directory.GetFiles(path + @"\common\", "*.txt", SearchOption.AllDirectories).ToList();
            int succes = 0;
            int failure = 0;
            foreach (string f in files)
            {
                try
                {
                    Parse(f);
                    succes++;
                } catch (Item.BadModException e)
                {
                    Debug.Log("Unable to parse " + f + " because of " + Environment.NewLine + "\t" + e.Message);
                    failure++;
                }
            }
            Debug.Log("Parsing: " + succes + " succeses and " + failure + " failures");
            //files.ConvertAll(f => Parse(f));
        }

        static public void ParseEvents(string path)
        {
            var files = Directory.GetFiles(path + @"\events\", "*.txt", SearchOption.AllDirectories).ToList();
            int succes = 0;
            int failure = 0;
            foreach (string f in files)
            {
                try
                {
                    Parse(f);
                    succes++;
                } catch (Item.BadModException e)
                {
                    Debug.Log("Unable to parse " + f + " because of " + Environment.NewLine + "\t" + e.Message);
                    failure++;
                }
            }
            Debug.Log("Parsing: " + succes + " succeses and " + failure + " failures");
            //files.ConvertAll(f => Parse(f));
        }

        /// <summary>
        /// This function takes a file and extracts a list of all the words, comments not included,
        /// together with their line location.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Item Parse(string path)
        {
            Debug.Log("Parsing " + path);
            List<string> lines = File.ReadLines(path).ToList();

            /// Step 1: subdivide the text into strings of either: 
            ///         -one word 
            ///         -open bracket '{' 
            ///         -close bracket '}' or 
            ///         -equality'='
            List<StringAndLoc> words = new List<StringAndLoc>();
            for (int i = 0; i < lines.Count; i++)
            {
                string currentWord = null;
                for (int j = 0; j < lines[i].Length; j++)
                {
                    char c = lines[i][j];
                    if (c == '#')
                        break;
                    if (c == ' ' || c == '\n' || c == '\t' || c == '\r')
                    {
                        if (currentWord != null)
                        {
                            words.Add(new StringAndLoc(currentWord, i + 1));
                            currentWord = null;
                        }
                    }
                    else if (c == '=' || c == '}' || c == '{')
                    {
                        if (currentWord != null)
                        {
                            words.Add(new StringAndLoc(currentWord, i + 1));
                            currentWord = null;
                        }
                        words.Add(new StringAndLoc(c.ToString(), i + 1));
                    }
                    else
                    {
                        if (currentWord == null)
                            currentWord = c.ToString();
                        else
                            currentWord += c;
                    }
                }

                if (currentWord != null)
                {
                    words.Add(new StringAndLoc(currentWord, i + 1));
                }

            }
            if(words.Count == 0)
            {
                throw new Item.BadModException(Item.ValueItem("NaN","NaN",path,0));
            }

            /// Step 2: Put the list of strings into a data structure
            /// This structure is nested items

            // Check if bracket count is right
            if (words.Count(s => s == "{") != words.Count(s => s == "}"))
            {
                throw new FormatException("The brackets in the file: " + path + " are not balanced. '{' is found " + words.Count(s => s == "{") +
                    " times while '}' is found " + words.Count(s => s == "}") + " times.");
            }

            return ExtractData(words,path);
        }

        /// <summary>
        /// Takes the list of words and converts it in an item, assuming the items starts
        /// at the word nr. startpoint.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="path"></param>
        /// <param name="startPoint"></param>
        /// <returns></returns>
        static Item ExtractData(List<StringAndLoc> words, string path, int startPoint = 0)
        {
            Item master = Item.ItemListItem(words[startPoint], path, words[startPoint].loc);
            if (words[startPoint + 1] != "=" || words[startPoint + 2] != "{")
                throw new Item.BadModException(master);
            int currentPos = startPoint + 3;
            while (currentPos < words.Count)
            {
                if (words[currentPos] == "}") // We have reached the end of this Item
                {
                    return master;
                }
                else if (words[currentPos + 2] == "{")
                {     // This is a new list item
                    if(words[currentPos + 4] == "=")
                    {   // This is a new item list item
                        master.AddItem(ExtractData(words, path, currentPos));
                        currentPos += 3;
                        int openBrackets = 1;
                        while (openBrackets > 0)    // Find the end of this section
                        {
                            if (words[currentPos] == "{") openBrackets++;
                            else if (words[currentPos] == "}") openBrackets--;
                            currentPos++;
                        }
                    } else
                    {   // This is a new value list item
                        Item val_list_it = Item.SimpleListItem(words[currentPos], path, words[currentPos].loc);
                        int i = 3;
                        while (words[currentPos+i] != "}")
                        {
                            if (words[currentPos + i] == "=" || words[currentPos + i] == "{")
                                throw new Item.BadModException(val_list_it);
                            val_list_it.AddValue(words[currentPos + i]);
                            i++;
                        }
                        currentPos += i + 1;
                        master.AddItem(val_list_it);
                    }
                }
                else // This is a new value item
                {
                    Item n = master.AddItem(words[currentPos], words[currentPos + 2], path, words[currentPos].loc);
                    if (words[currentPos + 1] != "=") throw new Item.BadModException(n);
                    currentPos += 3;
                }
            }
            throw new Item.BadModException(master); // The function should terminate within the loop
        }

        static public Item RetriveMasterItem(string name)
        {
            Item master = null;
            for (int i = 0; i < modData.Count; i++)
            {
                if (modData[i].name == name)
                    master = master == null ? modData[i] : master.Merge(modData[i]);
            }
            return master;
        }

        /// <summary>
        /// Container class for a combination of a string and an int.
        /// Is an implicite string
        /// </summary>
        class StringAndLoc
        {
            public readonly string s;
            public readonly int loc;
            public StringAndLoc(string s, int loc) { this.s = s; this.loc = loc; }
            public static implicit operator string(StringAndLoc sl) { return sl.s; }
        }

        /// <summary>
        /// An element in a mod file. It contains either a value or a list with Items
        /// </summary>
        public class Item
        {
            public readonly string name;
            string value;
            List<string> valueList;
            List<Item> itemList;
            enum ItemType { VALUE, SIMPLE_LIST, ITEM_LIST}
            ItemType itemType;
            public readonly Location location;

            Item(string name, string path, int line)
            {
                this.name = name;
                location = new Location() { path = path, line = line };
            }

            public static Item ValueItem(string name, string value, string path, int line)
            {
                return new Item(name, path, line) {
                    value = value,
                    itemType = ItemType.VALUE
                };
            }

            public static Item SimpleListItem(string name, string path, int line)
            {
                return new Item(name, path, line) {
                    valueList = new List<string>(),
                    itemType = ItemType.SIMPLE_LIST
                };
            }

            public static Item ItemListItem(string name, string path, int line)
            {
                return new Item(name, path, line) {
                    itemList = new List<Item>(),
                    itemType = ItemType.ITEM_LIST
                };
            }

            public void AddValue(string name)
            {
                if (itemType != ItemType.SIMPLE_LIST) throw new BadModException(this);
                valueList.Add(name);
            }

            public Item AddItem(string name, string value, string path, int line)
            {
                if (itemType != ItemType.ITEM_LIST) throw new BadModException(this);
                Item ret = ValueItem(name, value, path, line);
                itemList.Add(ret);
                return ret;
            }

            public Item AddItem(Item item)
            {
                if (itemType != ItemType.ITEM_LIST) throw new BadModException(this);
                itemList.Add(item);
                return item;
            }

            internal Item Merge(Item item)
            {
                if (this == null) return item;
                if (item == null) return this;
                if (item.itemType == ItemType.VALUE || this.itemType == ItemType.VALUE)
                    throw new Exception("You are trying to merge value items. You can only merge list items");
                if (item.itemType == ItemType.ITEM_LIST && itemType == ItemType.ITEM_LIST)
                {
                    Item merge = new Item(this.name, null, 0) {
                        itemType = ItemType.ITEM_LIST,
                        itemList = new List<Item>(this.itemList)
                    };
                    foreach (Item it in item.itemList)
                    {
                        merge.itemList.RemoveAll(i => i.name == it.name);
                        merge.itemList.Add(it);
                    }
                    return merge;
                } else if (item.itemType == ItemType.SIMPLE_LIST && itemType == ItemType.SIMPLE_LIST)
                {
                    Item merge = new Item(this.name, null, 0) {
                        itemType = ItemType.SIMPLE_LIST,
                        valueList = new List<string>(this.valueList)
                    };
                    foreach (string val in item.valueList)
                    {
                        merge.valueList.RemoveAll(v => v == val);
                        merge.valueList.Add(val);
                    }
                    return merge;
                } else
                    throw new Exception("You are trying to merge a value list item with an item list item.");
            }

            internal List<Item> GetChilderen()
            {
                if (itemType != ItemType.ITEM_LIST) throw new Exception("You cannot retrive the childeren of a value object");
                return itemList;
            }

            internal string GetString()
            {
                if (itemType != ItemType.VALUE) throw new Exception("You cannot retrive a value from a list item");
                return value;
            }

            internal double GetNumber()
            {
                if (itemType != ItemType.VALUE) throw new Exception("You cannot retrive a value from a list item");
                return double.Parse(value);
            }

            internal bool GetBool()
            {
                if (itemType != ItemType.VALUE) throw new Exception("You cannot retrive a value from a list item");
                return bool.Parse(value);
            }

            /// <summary>
            /// Returns the enum value of this item.
            /// BEWARE: this function will do no exception handeling
            /// </summary>
            /// <typeparam name="T">This is the type of the Enum</typeparam>
            /// <returns></returns>
            internal T GetEnum<T>()
            {
                if (itemType != ItemType.VALUE) throw new Exception("You cannot retrive a value from a list item");
                return (T)Enum.Parse(typeof(T), value);
            }

            internal Item GetItem(string valueName)
            {
                if (itemType != ItemType.ITEM_LIST) throw new Exception("This function must be used on a list item");
                return itemList.Find(i => i.name == valueName);
            }

            internal string GetString(string valueName)
            {
                if (itemType != ItemType.ITEM_LIST) throw new Exception("This function must be used on a list item");
                if (itemList.Any(i => i.name == valueName))
                    return itemList.Find(i => i.name == valueName).GetString();
                return "";
            }

            internal double GetNumber(string valueName)
            {
                if (itemType != ItemType.ITEM_LIST) throw new Exception("This function must be used on a list item");
                if (itemList.Any(i => i.name == valueName))
                    return itemList.Find(i => i.name == valueName).GetNumber();
                return 0;
            }

            internal bool GetBool(string valueName, bool _default = false)
            {
                if (itemType != ItemType.ITEM_LIST) throw new Exception("This function must be used on a list item");
                if (itemList.Any(i => i.name == valueName))
                    return itemList.Find(i => i.name == valueName).GetBool();
                return _default;
            }

            internal T GetEnum<T>(string valueName)
            {
                if (itemType != ItemType.ITEM_LIST) throw new Exception("This function must be used on a list item");
                if (itemList.Any(i => i.name == valueName))
                    return itemList.Find(i => i.name == valueName).GetEnum<T>();
                return default(T);
            }



            public struct Location
            {
                public string path;
                public int line;
            }


            [Serializable]
            public class BadModException : Exception
            {
                public BadModException(Item item) : base("There is a problem in file " + item.location.path + " around line " + item.location.line + ".") { }
                protected BadModException(
                  System.Runtime.Serialization.SerializationInfo info,
                  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            }
        }
    }
}
