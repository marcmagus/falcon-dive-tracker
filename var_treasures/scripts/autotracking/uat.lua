-- UAT example pack by black_sliver
-- autotracking.lua

-- For this demo we named the item codes and location section identical to the game variables.
-- Note that codes and variable names are case sensitive.
--
-- The return value of :ReadVariable can be anything, so we check the type and
-- * for toggles accept nil, false, integers <= 0 and empty strings as `false`
-- * for consumables everything that is not a number will be 0
-- * for progressive toggles we expect json [bool,number] or [number,number]
-- * for chests this is left as an exercise for the reader
-- Alternatively try-catch (pcall) can be used to handle unexpected values.

function updateToggles(store, vars)
    print("updateToggles")
    for _, var in ipairs(vars) do
        local o = Tracker:FindObjectForCode(var)
        local val = store:ReadVariable(var)
        if type(val) == "number" then; o.Active = val > 0
        elseif type(val) == "string" then; o.Active = val ~= ""
        else; o.Active = not(not val)
        end
        print(var .. " = " .. tostring(val) .. " -> " .. tostring(o.Active))
    end
end

-- When these toggles are updated also store the relevant value to the linked toggle
linkedtoggles = {
    ["boss-yang"] = "boss-baron-guards",
    ["boss-golbez"] = "boss-calcobrena",
}
function updateLinkedToggles(store, vars)
    print("updateLinkedToggles")
    for _, var in ipairs(vars) do
        local var2 = linkedtoggles[var]
        local o = Tracker:FindObjectForCode(var2)
        local val = store:ReadVariable(var)
        if type(val) == "number" then; o.Active = val > 0
        elseif type(val) == "string" then; o.Active = val ~= ""
        else; o.Active = not(not val)
        end
        print(var .. " -> " .. var2 .. " = " .. tostring(val) .. " -> " .. tostring(o.Active))
    end
end

function updateConsumables(store, vars)
    print("updateConsumables")
    for _, var in ipairs(vars) do
        local o = Tracker:FindObjectForCode(var)
        local val = store:ReadVariable(var)
        if type(val) == "number" then; o.AcquiredCount = val
        else; o.AcquiredCount = 0
        end
        print(var .. " = " .. tostring(val) .. " -> " .. o.AcquiredCount)
    end
end

function updateProgressives(store, vars)
    print("updateProgressives")
    for _, var in ipairs(vars) do
        local o = Tracker:FindObjectForCode(var)
        local val = store:ReadVariable(var)
        if type(val) == "number" then; o.CurrentStage = val
        else; o.CurrentStage = 0
        end
        print(var .. " = " .. tostring(val) .. " -> " .. o.CurrentStage)
    end
end


-- Table of codes which contribute to a countable progressive
-- Their stage will be set to the sum of these.
countables = {
    ["nothings"] = {"nothing-1","nothing-2","nothing-3","nothing-4","nothing-5","nothing-6","nothing-7","nothing-8","nothing-9"},
    ["crystal-shards"] = {"crystal-shard-1","crystal-shard-2","crystal-shard-3","crystal-shard-4"},
}

function updateCountableProgressives(store, vars)
    print("updateCountableProgressives")
    for _, var in ipairs(vars) do
        -- find the item matching this code
        local countable
        for k,v in pairs(countables) do
            for _,i in pairs(countables[k]) do
                if i == var then
                    countable = k
                end
            end
        end
        print("  - " .. tostring(var) .. " is " .. tostring(countable))
        -- count how many of our items were reported found
        local count = 0
        for _,i in ipairs(countables[countable]) do
            local val = store:ReadVariable(i)
            if type(val) == "number" then; count = count + val
            elseif type(val) == "string" then
                count = count + val ~= "" and 1 or 0
            else; count = count + not(not val) and 1 or 0
            end
        end
        local o = Tracker:FindObjectForCode(countable)
        o.CurrentStage = count
        print(var .. " = " .. tostring(countable) .. " -> " .. o.CurrentStage)
    end
end

function updateProgressiveToggles(store, vars)
    print("updateProgressiveToggles")
    for _, var in ipairs(vars) do
        local o = Tracker:FindObjectForCode(var)
        local val = store:ReadVariable(var)
        if type(val) == "table" and type(val[2]) == "number" then
            if type(val[1]) == "number" then; o.Active = val[1]>0
            else; o.Active = not(not val[1])
            end
            o.CurrentStage = val[2]
        else
            o.Active = false
        end
        print(var .. " = " .. tostring(val) .. " -> " .. tostring(o.Active) .. "," .. o.CurrentStage)
    end
end

function updateLocations(store, vars)
    print("updateLocations")
    -- if the variable is not named the same as the location
    -- you'll have to map them to actual section names
    -- if you use one boolean per chest
    -- you'll have to sum them up or remember the old value
    for _, var in ipairs(vars) do
        local o = Tracker:FindObjectForCode("@"..var) -- grab section
        local val = store:ReadVariable(var)
        o.AvailableChestCount = o.ChestCount - val -- in this case val = that many chests are looted
        print(var .. " = " .. tostring(val) .. " -> " .. tostring(o.AvailableChestCount))
    end
end

ScriptHost:AddVariableWatch("toggles", {
    -- "character-2",
    -- "character-3",
    -- "character-4",
    -- "character-5",
    "bomb-ring",
    "kaipo-pass",
    "legend-sword",
    "sand-ruby",
    "baron-key",
    "twin-harp",
    "earth-crystal",
    "magma-rock",
    "lugae-key",
    "hovercraft",
    "lucas-necklace",
    "dark-crystal",
    "rat-tail",
    "pink-tail",
    -- "trash-can",
    "frying-pan",
    "adamantite",
    "boss-mist-dragon",
    "boss-rydia",
    "boss-baron-soldiers",
    "boss-waterhag",
    "boss-octomamm",
    "boss-edward",
    "boss-antlion",
    "boss-mombomb",
    "boss-fabul-gauntlet",
    "boss-scarmiglione",
    "boss-scarmiglione-z",
    "boss-dkcecil",
    "boss-baron-guards",
    "boss-yang",
    "boss-baigan",
    "boss-cagnazzo",
    "boss-odin",
    "boss-dark-elf",
    "boss-magus-sisters",
    "boss-barbariccia",
    "boss-rat-tail",
    "boss-nothing-machine",
    "boss-calcobrena",
    "boss-golbez",
    "boss-lugae",
    "boss-dark-imps",
    "boss-asura",
    "boss-leviathan",
    "boss-demon-wall",
    "boss-sylvan-cave",
    "boss-king-queen-eblan",
    "boss-rubicante",
    "boss-bahamut",
    "boss-elements",
    "boss-cpu",
    "boss-white-dragon",
    "boss-dark-bahamut",
    "boss-plague",
    "boss-lunasaur",
    "boss-ogopogo",

}, updateToggles)
ScriptHost:AddVariableWatch("linkedtoggle", {"boss-yang", "boss-golbez"}, updateLinkedToggles)
-- ScriptHost:AddVariableWatch("consumables", {"b"}, updateConsumables)
-- ScriptHost:AddVariableWatch("progressive", {
--     "crystal-shards",
--     "nothings",
-- }, updateProgressives)
ScriptHost:AddVariableWatch("countable", {
    "crystal-shard-1",
    "crystal-shard-2",
    "crystal-shard-3",
    "crystal-shard-4",
    "nothing-1",
    "nothing-2",
    "nothing-3",
    "nothing-4",
    "nothing-5",
    "nothing-6",
    "nothing-7",
    "nothing-8",
    "nothing-9",
}, updateCountableProgressives)
ScriptHost:AddVariableWatch("locations", {
    "Mist Cave/Mist Dragon",
    "Mist Cave/Mist Cave [2-5]",
    "Mist/Rydia",
    "Mist/Mist [2-6]",
    "Kaipo/Kaipo Inn",
    "Kaipo/Kaipo Sand Ruby",
    "Kaipo/Kaipo, Oasis of the Desert [1-4]",
    "Underground Waterway/Underground Waterway [2-5]",
    "Underground Waterway/Hidden Chamber [3-6]",
    "Subterranean Lake/Octomammoth",
    "Subterranean Lake/Subterranean Lake [3-6]",
    "Damcyan Castle/Edward",
    "Damcyan Castle/Damcyan Castle [1-4]",
    "Antlion Cave/Antlion",
    "Antlion Cave/Antlion Cave [2-5]",
    "Antlion Cave/Antlion Cave [3-6]",
    "Mt. Hobs/Mom Bomb",
    "Mt. Hobs/Mt. Hobs [3-6]",
    "Fabul Castle/Fabul Gauntlet",
    "Fabul Castle/Fabul Castle [1-4]",
    "Fabul Castle/Fabul Castle [2-5]",
    "Mythril/Mythril [2-5]",
    "Mt. Ordeals/Scarmiglione - no reward",
    "Mt. Ordeals/Scarmiglione Z - no reward",
    "Mt. Ordeals/Dark Knight Cecil",
    "Mt. Ordeals/Mt. Ordeals [3-5]",
    "Town of Baron/Guards - no reward",
    "Town of Baron/Yang",
    "Town of Baron/Town of Baron [1-4]",
    "Town of Baron/Town of Baron [3-6]",
    "Town of Baron/Old Waterway [3-6]",
    "Town of Baron/Old Waterway [4-7]",
    "Castle Baron/Baigan - no reward",
    "Castle Baron/Cagnazzo",
    "Castle Baron/Odin",
    "Castle Baron/Castle Baron [3-6]",
    "Magnetic Cavern/Dark Elf",
    "Magnetic Cavern/Magnetic Cavern [3-6]",
    "Troia/Magus Sisters - no reward",
    "Troia/Barbariccia",
    "Troia/Tower of Zot [3-6]",
    "Troia/Tower of Zot Monster [4-7]",
    "Troia/Town of Troia [2-5]",
    "Troia/Troia Castle [2-6]",
    "Troia/Troia Castle - Treasury [5-9]",
    "Adamant Grotto/Rat Tail",
    "Adamant Grotto/Pink Tail [9]",
    "Agart/Nothing Vending Machine",
    "Agart/Nothing Vending Machine [9]",
    "Agart/Agart [2-5]",
    "Eblan Castle/Eblan Castle [2-5]",
    "Eblan Castle/Eblan Castle Monster [3-6]",
    "Dwarven Castle/Calcobrena - no reward",
    "Dwarven Castle/Golbez",
    "Dwarven Castle/Dwarven Castle [3-6]",
    "Dwarven Castle/Dwarven Castle [4-7]",
    "Tomra/Tomra [4-6]",
    "Kokkol's Smithy/Forge Excalibur [9]",
    "Kokkol's Smithy/Kokkol's Smithy [4-6]",
    "Lower Babel/Dr. Lugae",
    "Lower Babel/Dark Imps",
    "Lower Babel/Tower of Babel [4-7]",
    "Lower Babel/Tower of Babel Monster [5-8]",
    "Cave of Summons/Asura",
    "Cave of Summons/Leviathan",
    "Cave of Summons/Cave of Summons [4-7]",
    "Cave of Summons/Cave of Summons Monster [5-8]",
    "Cave of Summons/Land of Summons [4-6]",
    "Cave of Summons/Land of Summons - Hidden Chamber [5-7]",
    "Sealed Cavern/Demon Wall",
    "Sealed Cavern/Sealed Cavern [5-8]",
    "Sylvan Cave/Sylphs",
    "Sylvan Cave/Sylvan Cave [4-7]",
    "Sylvan Cave/Sylvan Cave Monster [5-8]",
    "Sylvan Cave/Hidden Chamber [6-9]",
    "Upper Babel/King and Queen Eblan - no reward",
    "Upper Babel/Rubicante - no reward",
    "Upper Babel/Eblan Settlement [3-6]",
    "Upper Babel/Cave of Eblan [4-6]",
    "Upper Babel/Cave of Eblan Monster [6-8]",
    "Upper Babel/Tower of Babel [4-7]",
    "Upper Babel/Tower of Babel Monster [6-9]",
    "Cave of Bahamut/Bahamut",
    "Cave of Bahamut/Cave of Bahamut [6-8]",
    "Western Lunar Path/Western Lunar Path [6-8]",
    "Western Lunar Path/Western Lunar Path Monster [7-9]",
    "Giant of Babel/Elemental Lords",
    "Giant of Babel/CPU",
    "Giant of Babel/Giant of Babel [5-8]",
    "Giant of Babel/Giant of Babel Monster [6-9]",
    "Lunar Subterrane/Murasame Altar",
    "Lunar Subterrane/Crystal Sword Altar",
    "Lunar Subterrane/White Spear Room",
    "Lunar Subterrane/Ribbon Room",
    "Lunar Subterrane/Masamune Altar",
    "Lunar Subterrane/Lunar Subterrane [6-8]",
    "Lunar Subterrane/Lunar Subterrane Monster [7-9]",
    "Lunar Subterrane/Lunar Subterrane - Core [7-9]",
    "Chocobo Forest, Baron Area/Chocobo Forest, Baron Area [2-4]",
    "Chocobo Forest, Fabul Area/Chocobo Forest, Fabul Area [3-5]",
    "Chocobo Forest, Mysidia Area/Chocobo Forest, Mysidia Area [2-4]",
    "Chocobo Forest, Troia Area/Chocobo Forest, Troia Area [4-6]",
    "Chocobo Forest, Southern Island/Chocobo Forest, Southern Island [4-6]",
    "Chocobo Village/Chocobo Village [3-6]",
}, updateLocations)
