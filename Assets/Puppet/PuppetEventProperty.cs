// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using UnityEngine.UI;

public class PuppetEventProperty : MonoBehaviour
{
    public InputField Key;
    public InputField Value;

    public void Remove()
    {
        Destroy(gameObject);
    }
}
