namespace DndWebApp.Api.Models;

public class Feat
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Prerequisite { get; set; }
    public required List<Race> Races { get; set; }
    public List<SkillProficiency> SkillProficiencies { get; set; } = [];
    public List<ChoiceOption<Skill>> SkillProficiencyChoices { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<ChoiceOption<Language>> LanguageChoices { get; set; } = [];
    public List<Spell> TraitSpells { get; set; } = [];
}

/**
"url": "https://api.open5e.com/v2/feats/a5e-ag_ace-driver/",
            "key": "a5e-ag_ace-driver",
            "has_prerequisite": true,
            "benefits": [
                {
                    "desc": "You gain an expertise die on ability checks made to drive or pilot a vehicle."
                },
                {
                    "desc": "While piloting a vehicle, you can use your reaction to take the Brake or Maneuver vehicle actions."
                },
                {
                    "desc": "A vehicle you load can carry 25% more cargo than normal."
                },
                {
                    "desc": "Vehicles you are piloting only suffer a malfunction when reduced to 25% of their hit points, not 50%. In addition, when the vehicle does suffer a malfunction, you roll twice on the maneuver table and choose which die to use for the result."
                },
                {
                    "desc": "Vehicles you are piloting gain a bonus to their Armor Class equal to half your proficiency bonus."
                },
                {
                    "desc": "When you Brake, you can choose to immediately stop the vehicle without traveling half of its movement speed directly forward."
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
            "name": "Ace Driver",
            "desc": "You are a virtuoso of driving and piloting vehicles, able to push them beyond their normal limits and maneuver them with fluid grace through hazardous situations. You gain the following benefits:\r\n* You gain an expertise die on ability checks made to drive or pilot a vehicle.\r\n* While piloting a vehicle, you can use your reaction to take the Brake or Maneuver vehicle actions.\r\n* A vehicle you load can carry 25% more cargo than normal.\r\n* Vehicles you are piloting only suffer a malfunction when reduced to 25% of their hit points, not 50%. In addition, when the vehicle does suffer a malfunction, you roll twice on the maneuver table and choose which die to use for the result.\r\n* Vehicles you are piloting gain a bonus to their Armor Class equal to half your proficiency bonus.\r\n* When you Brake, you can choose to immediately stop the vehicle without traveling half of its movement speed directly forward.",
            "prerequisite": "Proficiency with a type of vehicle",
            "type": "GENERAL"
        },
        {
            "url": "https://api.open5e.com/v2/feats/a5e-ag_athletic/",
            "key": "a5e-ag_athletic",
            "has_prerequisite": false,
            "benefits": [
                {
                    "desc": "Your Strength or Dexterity score increases by 1, to a maximum of 20."
                },
                {
                    "desc": "When you are prone, standing up uses only 5 feet of your movement (instead of half)."
                },
                {
                    "desc": "Your speed is not halved from climbing."
                },
                {
                    "desc": "You can make a running long jump or a running high jump after moving 5 feet on foot (instead of 10 feet)."
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
            "name": "Athletic",
            "desc": "Your enhanced physical training grants you the following benefits:\r\n\r\n* Your Strength or Dexterity score increases by 1, to a maximum of 20.\r\n* When you are prone, standing up uses only 5 feet of your movement (instead of half).\r\n* Your speed is not halved from climbing.\r\n* You can make a running long jump or a running high jump after moving 5 feet on foot (instead of 10 feet).",
            "prerequisite": "",
            "type": "GENERAL"
        },
**/