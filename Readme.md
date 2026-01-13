# ♟️ Simple Chess – Unity Project

A modern chess game built with **Unity UI**, designed for **offline play now** and **online multiplayer (future)** using **Photon PUN2**.

This README explains the **complete app flow**, **scene structure**, **UI logic**, and **multiplayer preparation**.

---

## 📱 APP FLOW (HIGH LEVEL)

App Launch
↓
Splash Scene (animation only)
↓
Main Menu Scene
↓
Offline Game / Friends Lobby / Profile

yaml
Copy code

- Splash screen is **visual only**
- All logic starts **after Main Menu loads**
- Global popups are available across scenes

---

## 🎬 SCENE FLOW

### 1️⃣ Splash Scene

**Purpose:** Branding + smooth entry

**What happens here**

- Logo animation
- Glow effects
- Footer text
- Scene auto-loads `MainMenuScene`

**Important**

- ❌ No popups shown here
- ✅ GlobalCanvas is CREATED here and marked `DontDestroyOnLoad`

---

### 2️⃣ Main Menu Scene

**Purpose:** Navigation hub

**Default State**

- HomePanel visible
- Footer tabs enabled
- Popups hidden

**On Scene Load**

1. Check Internet connection
2. If internet is ON:
   - Connect to Photon
   - If player name not set → show Name Popup
3. If internet is OFF:
   - Allow Offline / AI play
   - Block Friends & Profile actions

---

### 3️⃣ Offline Game Scene

**Purpose:** Full chess gameplay (local)

**Includes**

- Chess board
- Turn system
- Timer (Chess Clock)
- Draw / Resign
- End game popup

**No Internet Required**

---

## 🌐 GLOBAL CANVAS (IMPORTANT)

Created in **Splash Scene**, persists forever.

### Contains:

- `PlayerNamePopup`
- `NoInternetPopup`

These popups:

- ❌ Never show in Splash
- ✅ Can show in Main Menu or later scenes

---

## 🧠 POPUP RULES (VERY IMPORTANT)

### 🧾 Player Name Popup

Shown when:

- Internet is available
- Player name is NOT set
- Main Menu has loaded

Not shown when:

- Internet is OFF
- Player is playing Offline or AI

---

### 🚫 No Internet Popup

Shown when:

- Player tries to open:
  - Friends tab
  - Lobby panel
  - Profile panel
- AND internet is not available

Buttons:

- Retry → checks connection again
- Exit App → quits application

---

## 🏠 MAIN MENU UI STRUCTURE

Canvas
└── SafeArea
├── HeaderBar
├── MainContentArea
│ ├── HomePanel
│ ├── ProfilePanel
│ ├── SettingsPanel
│ └── LobbyPanel
├── FooterBar
└── PlayerNamePopup (modal)

yaml
Copy code

---

## 🏠 Home Panel

Main play entry

- Play Offline → Opens Offline Game
- Play with Friends → Opens Lobby (internet required)
- Play vs Computer → Future AI mode

---

## 👤 Profile Panel

- Static avatar
- Editable player name
- Save button

(Name is saved locally and synced to Photon nickname)

---

## 👥 Lobby Panel (Friends Mode – UI Only)

_Prepared for Photon Multiplayer_

Includes:

- Player info bar
- Room code (create / join)
- Player slots
- Start game (host only)

> No networking logic yet — UI only

---

## 🧭 FOOTER NAVIGATION

Footer Tabs:

- Home
- Friends
- Profile

Rules:

- Home → Always accessible
- Friends / Profile → Require internet + player name

Active tab:

- Button image visible
- Text color: `#F8FAFC`

Inactive tab:

- Button image alpha = 0
- Text color: `#9CA3AF`

---

## ♟️ GAMEPLAY FLOW (OFFLINE)

1. White starts
2. Select piece → show legal moves
3. Move piece → animate
4. Update turn
5. Check / Checkmate / Stalemate detection
6. Timer updates per turn
7. End game popup on finish

---

## ⏱️ CHESS CLOCK

- Separate timer for White & Black
- Starts automatically
- Stops on:
  - Checkmate
  - Draw
  - Resign
  - Time up

---

## 🏁 END GAME OPTIONS

Popup shows:

- Result text
- Restart button
- Exit button

---

## 🧩 SCRIPT ARCHITECTURE

Assets/Scripts
├── Core → App-wide systems
├── Splash → Splash screen only
├── Global → Popups, player data, internet
├── MainMenu → Menu UI & controllers
├── Game → Chess gameplay
└── Shared → Reusable utilities (future)

yaml
Copy code

---

## 🌍 MULTIPLAYER PLAN (FUTURE)

Using **Photon PUN2**

Planned:

- Player nickname sync
- Room create / join
- Move-based sync (not full board)
- Lobby ready UI

Not implemented yet.

---

## ✅ CURRENT STATUS

✔ Offline Chess Complete  
✔ Clean UI architecture  
✔ Global popup system  
✔ Internet-aware UX  
✔ Multiplayer-ready structure

---

## 🚀 NEXT STEPS

- Photon Lobby logic
- Room creation / join
- PlayerSlot syncing
- Online move synchronization

---

**Built with ❤️ in Unity**
