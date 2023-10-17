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
    -- "crystal-shard-1",
    -- "crystal-shard-2",
    -- "crystal-shard-3",
    -- "crystal-shard-4",
    -- "nothing-1",
    -- "nothing-2",
    -- "nothing-3",
    -- "nothing-4",
    -- "nothing-5",
    -- "nothing-6",
    -- "nothing-7",
    -- "nothing-8",
    -- "nothing-9"
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
-- ScriptHost:AddVariableWatch("consumables", {"b"}, updateConsumables)
ScriptHost:AddVariableWatch("progressive", {
    "crystal-shards",
    "nothings",
}, updateProgressives)
--ScriptHost:AddVariableWatch("locations", { }, updateLocations)
