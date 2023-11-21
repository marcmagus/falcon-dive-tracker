-- Utility
function characterCount()
    return Tracker:ProviderCountForCode("character-1") + Tracker:ProviderCountForCode("character-2") + Tracker:ProviderCountForCode("character-3") + Tracker:ProviderCountForCode("character-4") + Tracker:ProviderCountForCode("character-5")
end

function magmaRockInLogic()
    return characterCount() >= 2
end

function hovercraftInLogic()
    return characterCount() >= 3
end

function moonInLogic()
    return characterCount() >= 3
end
