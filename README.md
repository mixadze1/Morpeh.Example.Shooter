# First Person Shooter – ECS Morpeh Example

This is a Unity FPS project demonstrating the use of the [Morpeh ECS framework](https://github.com/scellecs/morpeh) to build scalable and modular shooter systems using data-driven architecture.

---

## 🔫 Shooting System

The project features a fully functional weapon shooting system, including:

- Fire with `ShootPerSecond` control
- Reloading with animation sync (`ReloadEnd` trigger)
- Bullet instantiation with speed and lifetime
- Hit detection via `Raycast`
- Decal spawning on impact with custom lifetime
- ECS Events for shooting and animation flow

### 🎬 Shooting Demo Video  
https://github.com/user-attachments/assets/5c948552-38a8-40a7-a43e-138276308580

###  Shooting Demo With Gizmos 

https://github.com/user-attachments/assets/28250565-afd4-4a46-8080-1e88b051e5d0

---

## 🔁 Weapon Switching

The player can switch between different weapons dynamically.  
Each weapon is:

- Defined via a `WeaponsConfig` asset
- Automatically applied to the player's current state
- Fully modular and extendable with new types

### Weapon Switching Demo Video  

https://github.com/user-attachments/assets/d490be13-36cf-448b-a1bb-bd2b05224023

---


## 🧠 Architecture Notes

This project uses **Morpeh version `2024.1`** — fully updated and free from obsolete APIs.

Key architecture choices:

- ✅ **No deprecated methods** — systems are written using the clean and modern `ISystem` interface
- ✅ **Pure ECS structure** — filters, stashes, and events are used to isolate data and logic
- ✅ **No tight coupling** — Unity code is limited to essential bridges like `MonoProvider` proxies

---

## ⚙️ Tech Stack

- Unity 2022.3.52
- [Morpeh ECS 2024.1](https://github.com/scellecs/morpeh)
- [Morpeh Events](https://github.com/codewriter-packages/Morpeh.Events)
- [VContainer](https://github.com/hadashiA/VContainer)
- Unity Input System
- ScriptableObjects for configuration

---
