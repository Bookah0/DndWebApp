using System.Collections.Generic;
using DndWebApp.Api.Utils;
using static DndWebApp.Api.Models.Class;

namespace DndWebApp.Api.Models;

// Based on https://api.open5e.com/v2/backgrounds/
public class Background
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<Race> Races { get; set; }
    public List<AbilityValue> AbilityIncreases { get; set; } = [];
    public List<SkillProficiency> SkillProficiencies { get; set; } = [];
    public List<ChoiceOption<Skill>> SkillProficiencyChoices { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<ChoiceOption<Language>> LanguageChoices { get; set; } = [];
    public List<Spell> TraitSpells { get; set; } = [];
}

/**
"url": "https://api.open5e.com/v2/backgrounds/a5e-ag_artisan/",
            "key": "a5e-ag_artisan",
            "benefits": [
                {
                    "name": "Ability Score Increases",
                    "desc": "+1 to Intelligence and one other ability score.",
                    "type": "ability_score"
                },
                {
                    "name": "Adventures And Advancement",
                    "desc": "If you participate in the creation of a magic item (a “master work”), you will gain the services of up to 8 commoner apprentices with the appropriate tool proficiency.",
                    "type": "adventures_and_advancement"
                },
                {
                    "name": "Connection and Memento",
                    "desc": "Roll 1d10, choose, or make up your own.\r\n\r\n### **Artisan Connections**\r\n\r\n1. The cruel master who worked you nearly to death and now does the same to other apprentices.\r\n2. The kind master who taught you the trade.\r\n3. The powerful figure who refused to pay for your finest work.\r\n4. The jealous rival who made a fortune after stealing your secret technique.\r\n5. The corrupt rival who framed and imprisoned your mentor.\r\n6. The bandit leader who destroyed your mentor’s shop and livelihood.\r\n7. The crime boss who bankrupted your mentor.\r\n8. The shady alchemist who always needs dangerous ingredients to advance the state of your art.\r\n9. Your apprentice who went missing.\r\n10. The patron who supports your work.\r\n\r\n### **Artisan Mementos**\r\n\r\n1. *Jeweler:* A 10,000 gold commission for a ruby ring (now all you need is a ruby worth 5,000 gold).\r\n2. *Smith:* Your blacksmith's hammer (treat as a light hammer).\r\n3. *Cook:* A well-seasoned skillet (treat as a mace).\r\n4. *Alchemist:* A formula with exotic ingredients that will produce...something.\r\n5. *Leatherworker:* An exotic monster hide which could be turned into striking-looking leather armor.\r\n6. *Mason:* Your trusty sledgehammer (treat as a warhammer).\r\n7. *Potter:* Your secret technique for vivid colors which is sure to disrupt Big Pottery.\r\n8. *Weaver:* A set of fine clothes (your own work).\r\n9. *Woodcarver:* A longbow, shortbow, or crossbow (your own work).\r\n10. *Calligrapher:* Strange notes you copied from a rambling manifesto. Do they mean something?",
                    "type": "connection_and_memento"
                },
                {
                    "name": "Equipment",
                    "desc": "One set of artisan's tools, traveler's clothes.",
                    "type": "equipment"
                },
                {
                    "name": "Skill Proficiencies",
                    "desc": "Persuasion, and either Insight or History.",
                    "type": "skill_proficiency"
                },
                {
                    "name": "Tool Proficiencies",
                    "desc": "One type of artisan’s tools or smith’s tools.",
                    "type": "tool_proficiency"
                },
                {
                    "name": "Trade Mark",
                    "desc": "When in a city or town, you have access to a fully-stocked workshop with everything you need to ply your trade. Furthermore, you can expect to earn full price when you sell items you have crafted (though there is no guarantee of a buyer).",
                    "type": "feature"
                }
            ],
            "document": {
                "name": "Adventurer's Guide",
                "key": "a5e-ag",
                "display_name": "Adventurer's Guide",
                "publisher": {
                    "name": "EN Publishing",
                    "key": "en-publishing",
                    "url": "https://api.open5e.com/v2/publishers/en-publishing/"
                },
                "gamesystem": {
                    "name": "Advanced 5th Edition",
                    "key": "a5e",
                    "url": "https://api.open5e.com/v2/gamesystems/a5e/"
                },
                "permalink": "https://a5esrd.com/a5esrd"
            },
            "name": "Artisan",
            "desc": "[No description provided]"
**/