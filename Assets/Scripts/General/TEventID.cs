using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TEventID
{
    OnHealthUpdate,

    //Used when a item was collected.
    OnItemCollected,

    //Used when a item was selected.
    OnItemSelected,

    //Used when a item was deselected.
    OnItemDeselected,

    //Used when a item was correctly equipped.
    OnItemEquipped,

    //Used when a item was correctly unequipped.
    OnItemUnequipped,

    //Used to open the inventory.
    OnInventoryOpen,

    //Used to select a slot in hotbar.
    OnSelectSlotWithNumber,

    //Triggered when a message is sent
    OnMessageSent,

    //Triggered when a collection text is disabled
    OnCollectionTextDisabled,

    //Triggered on Tile damaging
    OnTileDamaged,
    OnTileDestroyed,

    //Triggered by the passing of time
    OnTimeStarted,
    OnMinutePassed,
    OnDayNightChanged,

    //Used when the inventory finished to be created.
    OnInventoryCreated,

    //Used to search a item into the collection.
    OnSearchItem,

    //Used to Open or Close the description.
    OnOpenCloseDescription,

    //Used to show the item's description.
    OnShowTextDescription,
    OnPlayerDied,
    OnSpawnPointSelected,
    OnSpawnPointRemoved,
    OnPlayerSpawned
}
