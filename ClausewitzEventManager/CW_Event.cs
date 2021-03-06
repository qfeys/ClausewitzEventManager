﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClausewitzEventManager.Scripting;
using ClausewitzEventManager.Common;

namespace ClausewitzEventManager
{
    class CW_Event
    {
        string name_space;
        int id;
        enum EventType { CHARACTER_EVENT, LONG_CHARACTER_EVENT, LETTER_EVENT, NARRATIVE_EVENT, PROVINCE_EVENT, DIPLORESPONSE_EVENT, UNIT_EVENT, SOCIETY_QUEST_EVENT }
        EventType eventType;

        ComplexTitle title;
        List<TriggeredDescription> descriptions;
        GFX_Link defaultPicture;
        GFX_Link border;
        // Sound - sound class?

        // Flags
        bool major;
        bool is_friendly;
        bool is_hostile;
        bool is_triggered_only;
        bool triggered_from_code;
        bool hide_from;
        bool hide_new;
        bool hide_window;
        bool show_root;
        bool show_from_from;
        bool show_from_from_from;
        bool notification;
        string portrait;
        enum OffMapModes { ONLY, ALLOW, NO }
        OffMapModes offmap;
        string window; // Used for Jade dragon. Only known valid parameter: EventWindowOffmap
        GFX_Link background; // Used only together with window
        string quest_target; // Used for SOCIETY_QUEST_EVENT

        // Pre-triggers
        // Filtering
        bool only_playable;
        bool is_part_of_plot;
        bool only_rulers;
        Religion religion;
        ReligionGroup religion_group;
        // Other
        int min_age;
        int max_age;
        bool only_independent;
        bool only_men;
        bool only_women;
        bool only_capable;
        List<string> lacks_dlc;
        List<string> has_dlc;
        bool friends;
        bool rivals;
        bool prisoner;
        bool ai;
        bool is_patrician;
        bool is_married;
        bool is_sick;
        string has_character_flag; // Only use one
        string has_global_flag; // Only use one
        string has_flag; // Less used
        bool war;
        Culture culture;
        CultureGroup culture_group;
        bool is_in_society;

        Trigger trigger;
        Trigger major_trigger;
        MeanTimeToHappen mtth;
        WieghtMultiplier weight_multiplier;

        Command immediate;
        Command fail_trigger_effect;
        Command after;

        List<Option> options;


        CW_Event(EventType eventType)
        {
            this.eventType = eventType;
            title = new ComplexTitle();
            descriptions = new List<TriggeredDescription>();
            lacks_dlc = new List<string>();
            has_dlc = new List<string>();
            options = new List<Option>();
        }

        static public CW_Event FromParsedItem(Parser.Item root)
        {
            CW_Event ev = new CW_Event((EventType)Enum.Parse(typeof(EventType), root.name, true));
            foreach (Parser.Item item in root.GetChilderen())
            {
                switch (item.name.ToLower())
                {
                case "id":
                    string id = item.GetString();
                    string[] x = id.Split('.');
                    if (x.Length == 2)
                    {
                        ev.name_space = x[0];
                        ev.id = int.Parse(x[1]);
                    } else
                    {
                        ev.name_space = null;
                        ev.id = int.Parse(id);
                    }
                    break;
                // Flags
                case "title":
                    ev.title.Add(item);
                    break;
                case "desc":
                    ev.descriptions.Add(new TriggeredDescription(item));
                    break;
                case "picture":
                    ev.defaultPicture = GFX_Link.FromTag(item.GetString());
                    break;
                case "border":
                    ev.border = GFX_Link.FromTag(item.GetString());
                    break;
                case "major": ev.major = item.GetBool(); break;
                case "is_friendly": ev.is_friendly = item.GetBool(); break;
                case "is_hostile": ev.is_hostile = item.GetBool(); break;
                case "is_triggered_only": ev.is_triggered_only = item.GetBool(); break;
                case "triggered_from_code": ev.triggered_from_code = item.GetBool(); break;
                case "hide_from": ev.hide_from = item.GetBool(); break;
                case "hide_new": ev.hide_new = item.GetBool(); break;
                case "hide_window": ev.hide_window = item.GetBool(); break;
                case "show_root": ev.show_root = item.GetBool(); break;
                case "show_from_from": ev.show_from_from = item.GetBool(); break;
                case "show_from_from_from": ev.show_from_from_from = item.GetBool(); break;
                case "sound": /*Find soound file or something*/ break;
                case "notification": ev.notification = item.GetBool(); break;
                case "portrait": ev.portrait = item.GetString(); break;
                case "offmap": ev.offmap = item.GetEnum<OffMapModes>(); break;
                case "window": ev.window = item.GetString(); break;
                case "background": ev.background = GFX_Link.FromTag(item.GetString()); break;
                case "quest_target": ev.quest_target = item.GetString();break;
                // Pre triggers
                case "only_playable": ev.only_playable = item.GetBool(); break;
                case "is_part_of_plot": ev.is_part_of_plot = item.GetBool(); break;
                case "only_rulers": ev.only_rulers = item.GetBool(); break;
                case "religion":
                    ev.religion = Religion.FromName(item.GetString());
                    break;
                case "religion_group":
                    ev.religion_group = ReligionGroup.FromName(item.GetString());
                    break;
                case "min_age": ev.min_age = (int)item.GetNumber(); break;
                case "max_age": ev.max_age = (int)item.GetNumber(); break;
                case "only_independent": ev.only_independent = item.GetBool(); break;
                case "only_men": ev.only_men = item.GetBool(); break;
                case "only_women": ev.only_women = item.GetBool(); break;
                case "only_capable": ev.only_capable = item.GetBool(); break;
                case "capable_only": ev.only_capable = item.GetBool(); break;
                case "lacks_dlc":
                    ev.lacks_dlc.Add(item.GetString());
                    break;
                case "has_dlc":
                    ev.has_dlc.Add(item.GetString());
                    break;
                case "friends": ev.friends = item.GetBool(); break;
                case "rivals": ev.rivals = item.GetBool(); break;
                case "prisoner": ev.prisoner = item.GetBool(); break;
                case "ai": ev.ai = item.GetBool(); break;
                case "is_patrician": ev.is_patrician = item.GetBool(); break;
                case "is_married": ev.is_married = item.GetBool(); break;
                case "is_sick": ev.is_sick = item.GetBool(); break;
                case "has_character_flag": ev.has_character_flag = item.GetString(); break;
                case "has_global_flag": ev.has_global_flag = item.GetString(); break;
                case "has_flag": ev.has_flag = item.GetString(); break;
                case "war": ev.war = item.GetBool(); break;
                case "culture":
                    ev.culture = Culture.FromName(item.GetString());
                    break;
                case "culture_group":
                    ev.culture_group = CultureGroup.FromName(item.GetString());
                    break;
                case "is_in_society": ev.is_in_society = item.GetBool(); break;
                // End of pre triggers
                case "trigger":
                    ev.trigger = new Trigger(item);
                    break;
                case "major_trigger":
                    ev.major_trigger = new Trigger(item);
                    break;
                case "mean_time_to_happen":
                    ev.mtth = new MeanTimeToHappen(item);
                    break;
                case "weight_multiplier":
                    ev.weight_multiplier = new WieghtMultiplier(item);
                    break;
                case "immediate":
                    ev.immediate = new Command(item);
                    break;
                case "fail_trigger_effect":
                    ev.fail_trigger_effect = new Command(item);
                    break;
                case "option":
                    ev.options.Add(new Option(item));
                    break;
                case "after":
                    ev.after = new Command(item);
                    break;
                default:
                    throw new Parser.Item.BadModException(item, "Did not recognise tag " + item.name);
                }
            }
            return ev;
        }


        public override string ToString()
        {
            if (title != null && title.Value != null && title.Value.Exists())
                return title.Value.Value;
            else
                return name_space + "." + id;
        }

        class TriggeredDescription
        {
            Trigger trigger;
            LocalisationLink text;
            GFX_Link picture;
            // sound ??

            public TriggeredDescription(Parser.Item root)
            {
                if (root.IsValue())
                    text = LocalisationLink.FromTag(root.GetString());
                else
                    foreach (Parser.Item item in root.GetChilderen())
                    {
                        switch (item.name)
                        {
                        case "trigger":
                            trigger = new Trigger(item);
                            break;
                        case "text":
                            text = LocalisationLink.FromTag(item.GetString());
                            break;
                        case "picture":
                            picture = GFX_Link.FromTag(item.GetString());
                            break;
                        case "sound":
                            // ???????
                            break;
                        default:
                            throw new Parser.Item.BadModException(item, "Did not recognise tag " + item.name);
                        }
                    }
            }
        }

        private class ComplexTitle
        {
            bool iscomplex = false;

            LocalisationLink simpleTitle;
            List<Tuple<LocalisationLink, Trigger>> titles;

            public LocalisationLink Value { get { return iscomplex ? titles[0].Item1 : simpleTitle; } }

            public ComplexTitle() { }

            public void Add(Parser.Item root)
            {
                if (iscomplex == false && simpleTitle == null && root.IsValue())
                    simpleTitle = LocalisationLink.FromTag(root.GetString());
                else if (iscomplex == false && simpleTitle != null && root.IsValue())
                {
                    iscomplex = true;
                    titles = new List<Tuple<LocalisationLink, Trigger>>() { new Tuple<LocalisationLink, Trigger>(simpleTitle,null),
                                                                            new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(root.GetString()),null)};
                } else if (iscomplex == false && simpleTitle == null && root.IsValue() == false)
                {
                    iscomplex = true;
                    titles = new List<Tuple<LocalisationLink, Trigger>>() {
                        new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(root.GetString("text")),new Trigger(root.GetItem("trigger"))) };
                } else if (iscomplex == false && simpleTitle != null && root.IsValue() == false)
                {
                    iscomplex = true;
                    titles = new List<Tuple<LocalisationLink, Trigger>>() {
                        new Tuple<LocalisationLink, Trigger>(simpleTitle,null),
                        new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(root.GetString("text")),new Trigger(root.GetItem("trigger")))
                    };
                } else if (iscomplex && root.IsValue())
                    titles.Add(new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(root.GetString()), null));
                else
                    titles.Add(new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(root.GetString("text")), new Trigger(root.GetItem("trigger"))));
            }
        }

        class Option
        {
            List<Tuple<LocalisationLink, Trigger>> name;
            Trigger trigger;
            Command command;
            float ai_base_chance;
            List<Tuple<float, Trigger>> ai_chance_modefiers;

            List<Scope> characterScopes;

            enum TooltipInfoOptions { MARTIAL , STEWARDSHIP , INTRIGUE , DIPLOMACY , LEARNING , CUSTOM}
            TooltipInfoOptions tooltip_info;
            Trait customTooltipInfo;
            private Parser.Item item;

            public Option(Parser.Item root)
            {
                name = new List<Tuple<LocalisationLink, Trigger>>();
                command = new Command();
                foreach (Parser.Item item in root.GetChilderen())
                {
                    switch (item.name)
                    {
                    case "name":
                        if (item.IsValue())
                            name.Add(new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(item.GetString()), null));
                        else
                            name.Add(new Tuple<LocalisationLink, Trigger>(LocalisationLink.FromTag(item.GetString("text")), new Trigger(item.GetItem("trigger"))));
                        break;
                    case "trigger":
                        trigger = new Trigger(item);
                        break;
                    default:
                        command.Add(item);
                        break;
                    }
                }
            }
        }
    }
}
