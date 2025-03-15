# Scripting Style Guide

## Modelling Objects

Whenever deriving a new `MonoBehaviour`, consider how that 
class would exist outside of Unity.

Imagine a prefab as a large class file, with each component
script being a new property or field.

### Example

`Player` has an `Inventory`, `Chopper` behaviour, `Health`.

Each of the three would be a separate component script. None of
these components should be aware of, or depend on another.

If necessary, they communicate indirectly via events and a
`Player` component script. Otherwise, refrain from creating a
`Player` component that does everything.