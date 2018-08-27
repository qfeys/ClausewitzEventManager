using System;
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

        LocalisationLink name;
        List<TriggeredDescription> descriptions;
        GFX_Link defaultPicture;
        // Border (use enum?)
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

        // Pre-triggers
        // Filtering
        bool only_playable;
        bool is_part_of_plot;
        bool only_rulers;
        Religion religion;
        ReligionGroup religionGroup;
        // Other
        int min_age;
        int max_age;
        bool only_independent;
        bool only_men;
        bool only_women;
        bool only_capable;
        string lacks_dlc;
        string has_dlc;
        bool friends;
        bool rivals;
        bool prisoner;
        bool ai;
        bool is_patrician;
        bool is_married;
        bool is_sick;
        string has_character_flag; // Only use one
        string has_global_flag; // Only use one
        bool war;
        Culture culture;
        CultureGroup culture_group;
        bool is_in_society;

        Trigger trigger;
        MeanTimeToHappen mtth;
        WieghtMultiplier weight_multiplier;

        Command immediate;
        Command after;

        Option option1;
        Option option2;
        Option option3;
        Option option4;


        class TriggeredDescription
        {
            Trigger trigger;
            LocalisationLink text;
            GFX_Link picture;
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
        }
    }
}
