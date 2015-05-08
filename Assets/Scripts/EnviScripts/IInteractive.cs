﻿using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

/*
 * Interface for all interactable objects, each have a function to perform when interact with
 */
public interface IInteractive
{
    void PerformPurpose(InventoryLogic inventory);
}