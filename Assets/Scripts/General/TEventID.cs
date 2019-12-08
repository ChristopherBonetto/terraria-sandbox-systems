using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TEventID
{
    OnPointerMovedOnGrid,
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

    
    OnCollectionTextDisabled,
    OnTileDamaged,
    OnTileDestroyed,
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
