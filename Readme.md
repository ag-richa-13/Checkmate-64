# ♟️ Simple Chess

A modern chess game built with **Unity UI**. The project currently supports **offline play** and is structured to be **multiplayer-ready** using **Photon PUN2** in future updates.

---

## 🚀 Quick Overview

- Platform: Unity (URP)
- Multiplayer: Prepared for Photon PUN2 (not yet implemented)
- Current status: **Offline chess complete**, clean UI architecture, global popup system, internet-aware UX

---

## 📱 App Flow (High Level)

1. App Launch
2. Splash Scene (visual only)
3. Main Menu Scene
4. Choose: Offline Game / Friends Lobby / Profile

Notes:

- The Splash scene is visual only; all logic starts after Main Menu loads.
- A global canvas is created at startup and persists across scenes.

---

## 🎬 Scene Flow

### Splash Scene

- Purpose: Branding + smooth entry
- Actions: Logo animation, glow effects, footer text
- Automatically loads `MainMenuScene`
- Important: No popups shown here; the `GlobalCanvas` is created and marked `DontDestroyOnLoad`.

### Main Menu Scene

- Purpose: Navigation hub
- Default: `HomePanel` visible, footer tabs enabled, popups hidden
- On load:
  1. Check Internet connection
  2. If internet is ON:
     - Connect to Photon
     - If player name is not set → show `PlayerNamePopup`
  3. If internet is OFF:
     - Allow Offline / AI play
     - Block Friends & Profile actions

### Offline Game Scene

- Purpose: Full chess gameplay (local)
- Features: Chess board, turn system, chess clock, draw/resign, end-game popup
- No Internet required

---

## 🌐 Global Canvas (Important)

Created in the Splash scene and persists for the whole session.

Contains:

- `PlayerNamePopup`
- `NoInternetPopup`

Rules:

- These popups never show in the Splash scene but may appear in Main Menu and later scenes.

---

## 🧠 Popup Rules

### Player Name Popup

Shown when:

- Internet is available
- Player name is NOT set
- Main Menu has loaded

Not shown when:

- Internet is OFF
- Player is in Offline or AI modes

### No Internet Popup

Shown when a user tries to open Friends, Lobby, or Profile while offline.
Buttons:

- **Retry**: re-check connection
- **Exit App**: quit application

---

## 🏠 Main Menu UI Structure

```
Canvas
└── SafeArea
    ├── HeaderBar
    ├── MainContentArea
    │   ├── HomePanel
    │   ├── ProfilePanel
    │   ├── SettingsPanel
    │   └── LobbyPanel
    ├── FooterBar
    └── PlayerNamePopup (modal)
```

---

## 🏠 Home Panel

- **Play Offline** → Opens Offline Game
- **Play with Friends** → Opens Lobby (internet required)
- **Play vs Computer** → Future AI mode

---

## 👤 Profile Panel

- Static avatar
- Editable player name
- Save button
- Name saved locally and synced to Photon nickname when connected

---

## 👥 Lobby Panel (Friends Mode — UI Only)

Prepared for Photon multiplayer; includes player info bar, room create/join field, player slots, and start game button (host only). NOTE: networking logic not implemented yet — UI only.

---

## 🧭 Footer Navigation

Tabs: Home, Friends, Profile

Rules:

- Home: always accessible
- Friends/Profile: require internet + player name
- Active tab: image visible, text color `#F8FAFC`
- Inactive tab: image alpha = 0, text color `#9CA3AF`

---

## ♟️ Gameplay Flow (Offline)

1. White starts
2. Select piece → show legal moves
3. Move piece → animate
4. Update turn
5. Detect check / checkmate / stalemate
6. Update timers per turn
7. Show end-game popup on finish

---

## ⏱️ Chess Clock

- Separate timers for White and Black
- Starts automatically
- Stops on: checkmate, draw, resign, or time up

---

## 🏁 End Game Options

- Result text
- Restart
- Exit

---

## 🧩 Script Architecture

```
Assets/Scripts
├── Core      → App-wide systems
├── Splash    → Splash-only logic
├── Global    → Popups, player data, internet checks
├── MainMenu  → Menu UI & controllers
├── Game      → Chess gameplay logic
└── Shared    → Reusable utilities
```

---

## 🌍 Multiplayer Plan (Future)

Using **Photon PUN2** (planned):

- Player nickname sync
- Room create / join
- Player slot syncing
- Move-based sync (not full board)
- Lobby ready UI

---

## ✅ Current Status

- ✔ Offline Chess: Complete
- ✔ Clean UI architecture
- ✔ Global popup system
- ✔ Internet-aware UX
- ✔ Multiplayer-ready project structure (UI)

---

## 🚀 Next Steps

1. Implement Photon Lobby logic
2. Room creation / join flows
3. Sync PlayerSlots and ready state
4. Networked move synchronization

---

**Built with ❤️ in Unity**
