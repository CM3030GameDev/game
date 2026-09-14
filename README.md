# City Z

A 2D top-down bullet heaven game made in Unity for CM3030 Games Development.

Fight through waves of robots with your squad, level up, pick upgrades, and combine weapons with stats to evolve them.

---

## Setting

The robots have taken over **City Z**. A lone Soldier, backed by a flamethrower-wielding Mercenary, pushes into the city to stop them once and for all.

| Act | Area | What happens |
|---|---|---|
| Tutorial | Outside the city | Learn to move, aim, fight and use skills. |
| Act 1 | City Outskirts | Push through the outskirts, secure the area, defeat the miniboss and meet the Swordsman, who joins the squad. |
| Act 2 | The City | Lure out and defeat the boss guarding the city generator, survive its overload, then destroy it to cut the city's power and open the gates. |
| Act 3 | Final Fight | Face the two-phase final boss at the heart of the city. |

### The squad
- **Soldier** (you). Fires automatically at the nearest enemy, or wherever you aim.
- **Mercenary** (companion). Burns nearby enemies with a flamethrower.
- **Swordsman** (companion, joins in Act 1). Cuts down enemies up close.

Companions fight on their own and stay close to the Soldier.

### Progression
- Enemies drop **EXP orbs**. Each level up pauses the game and offers upgrade cards.
- Pick **secondary weapons** (Machine Gun, Shotgun, Bazooka, Mines, EMP Field, Combat Drones) and **stats** (Health, Move Speed, Fire Rate, Damage, Pickup Range, Health Regen).
- Max a weapon and its paired stat to **combine** them into an evolved weapon. Green cards show a matching pair.
- Your main weapon upgrades automatically as you level.
- Enemies sometimes drop **health** and **magnet** pickups.
- Upgrades carry over between acts. If you die, you respawn at the start of the current area.

---

## Controls

| Action | Key |
|---|---|
| Move | W A S D |
| Toggle auto / manual aim | Left Click |
| Aim (manual mode) | Mouse |
| Air Strike (Soldier skill) | Right Click |
| Smokescreen (Mercenary skill, slows all enemies on screen) | 1 |
| Guard (Swordsman skill, brief invulnerability and speed boost) | 2 |
| Pause / back | Esc |

Skills recharge over time, and kills shorten the wait. Hover a skill icon in the bottom right to see what it does.

---

## How to run

1. Open the project in **Unity 6000.3.14f1**.
2. Open `Assets/Scenes/StartMenu.unity` and press Play.

Scene order: StartMenu, Tutorial, Act1, Act4 (Act 2), Act3, Act3_Phase2, Credits.

---

## Credits

### Team
| Part | Member |
|---|---|
| Tutorial | Nicholas |
| Act 1 | Harold |
| Act 2 | Kang Kang |
| Act 3 | Hong Shuai |

### Created by the team
| Asset | Creator | Used for |
|---|---|---|
| Stats Sprites | Harold | Stat icons, upgrade panel |
| Combat Drone | Harold | Weapon |

### Visual assets
| Asset | Creator | Source | Used for |
|---|---|---|---|
| Animated City Background | owmyknees | owmyknees.itch.io | Level backgrounds |
| Buildings & Wirefence | 0_mem0ry | 0-mem0ry.itch.io | City environment tiles |
| Doors | Joao9396 | joao9396.itch.io | Environment, level transitions |
| Road | GuttyKreum | guttykreum.itch.io | City environment tiles |
| Factory Asset v.2 Teaser | Blood_seller | blood-seller.itch.io/factory-asset-v2-teaser | Environment props |
| PixelVehicles | Minzinn | minzinn.itch.io/pixelvehicles | Environment props |
| Soldier, Mercenary, Swordsman | Gif (@gif_not_jif), Noiracide (@Noiracide), Romi (@DessRomaric) | Twitter | Playable characters |
| Enemy Robotic Dogs | Silver Ink | silverink.itch.io | Standard enemies |
| Act 1 Boss | Fly | floatingkites.itch.io | Act 1 boss |
| Act 2 Boss | Art man oil | art-man-oil.itch.io | Act 2 boss |
| Act 3 Final Boss Phase 1 | Emcee Flesher | opengameart.org/users/emcee-flesher | Final boss phase 1 |
| Act 3 Final Boss Phase 2 | Elthen | elthen.itch.io | Final boss phase 2 |
| Weapons Pack | Jestan | jestan.itch.io/weapons-pack | Weapon sprites |
| Modern Weapon Pack | lapoulemexicaine | lapoulemexicaine.itch.io/modern-weapon-pack | Weapon sprites |
| Fire Pixel Bullet 16x16 | BDragon1727 | bdragon1727.itch.io/fire-pixel-bullet-16x16 | Projectile sprites |
| Bullet Impact Explosion 32x32 | BDragon1727 | bdragon1727.itch.io/free-effect-bullet-impact-explosion-32x32 | Bullet impact VFX |
| VFX Free Pack | CodeManu | codemanu.itch.io/vfx-free-pack | Skill VFX |
| RPG Ability Icons | Frosty Rabbid | frosty-rabbid.itch.io/rpg-ability-icons | Skill icons |
| Stone UI Free Asset | Canvas Coven | canvas-coven.itch.io/stone-ui-free-asset | GUI panels, level up panel |
| Keyboard & Mouse UI | goncalomcoliveira | goncalomcoliveira.itch.io/keyboard-mouse-ui | Control prompts |
| Basic Pixel Health Bar & Scroll Bar | BDragon1727 | bdragon1727.itch.io/basic-pixel-health-bar-and-scroll-bar | Health and EXP bars |

### Audio
| Asset | Creator | Source | Used for |
|---|---|---|---|
| 6 Dark Fantasy Boss Battle Tracks | alkakrab | Unity Asset Store | Boss fight music |
| 400 Sounds Pack | ci | ci.itch.io/400-sounds-pack | Pickups, UI, level up |
| PUNCH-BOXING-02 | newagesoup (Freesound) | Pixabay | Melee hit |
| 062708 Laser Charging | Freesound Community | Pixabay | Skill charge up |
| Magic Chargeup | cribbler (Freesound) | Pixabay | Skill charge up |
| Whoosh Motion | DRAGON-STUDIO | Pixabay | Dash |
| Laser | Ahmed_Abdulaal | Pixabay | Laser attack |
| Fire Spell Impact | DRAGON-STUDIO | Pixabay | Fire skill impact |
| Cinematic Low Hit | Universfield | Pixabay | Boss impact |
| Metal Slam 5 | floraphonic | Pixabay | Robot / door impact |

### Act 3 map
| Asset | Creator | Source |
|---|---|---|
| Water | SciGho | ninjikin.itch.io |
| Bridge | Reemax | opengameart.org/users/reemax |
| Road | Faufilage | opengameart.org/users/faufilage |
| Laboratory | Hyptosis | hyptosis.itch.io |
| Lab | marceles | opengameart.org/users/marceles |
| Plaza | n2liquid | opengameart.org/content/exterior-32x32-town-tileset |
| Buildings | AO85, TomothyCreates, 0_mem0ry | ao-85.itch.io, ttomothyy.itch.io, 0-mem0ry.itch.io |
| Building | William Thompsonj, Sharm | opengameart.org/content/lpc-terrain-repack |
| Scrap & Junk | Haydeos | haydeos.itch.io |
| Pillar | InThePixel | opengameart.org/users/inthepixel |

### Act 3 visual assets
| Asset | Creator | Source |
|---|---|---|
| Villain | Gif (@gif_not_jif), Noiracide (@Noiracide), Romi (@DessRomaric) | Twitter |
| Robot Mobs | ashenremains | ashenremains.itch.io |
| Status Effect | Nockzoo | nockzoo.itch.io |
| Keyboard E Button | MegaCrash | megacrash.itch.io |
| Hydrant | thekingphoenix | opengameart.org/users/thekingphoenix |
| Hydrant Water | Foozle | foozlecc.itch.io |
| Hydrant Puddle | Diarandor | deviantart.com/diarandor/gallery |
| Flame Circle | Frostwindz | frostwindz.itch.io |
| Exhaust | KlenchAndrei | klenchandrei.itch.io |
| Homing Missile | diggy | opengameart.org/users/diggy |
| Missile Explosion | ansimuz | ansimuz.itch.io |
| Missile Barrage | Silas Games | silasgamedev.itch.io |
| Missile Barrage Explosion | unTied Games | untiedgames.itch.io |
| Eye Laser | caeden_8o8gamestudio | caeden-8o8gamestudio.itch.io |
| Suction | Julien | opengameart.org/users/julien |
| Shockwave | EmiEmiGames | emiemigames.itch.io |
| Ground Smash | nerijs | nerijs.itch.io |
| Ground Impact | unTied Games | untiedgames.itch.io |
| Fire Cannon Shoot | a_klingon | aklingon.itch.io |
| Fire Cannon Explosion | pimen | pimen.itch.io |
| Lightning Bolt | ansimuz | ansimuz.itch.io |
| Air Blast | Cethiel | opengameart.org/users/cethiel |
| Fireball | NYKNCK | nyknck.itch.io |
| Barrier | GameProgrammingSlave | opengameart.org/users/gameprogrammingslave |
| Target | Learn GameMaker | learngamemaker.itch.io |
| Smoke Screen | pimen | pimen.itch.io |
| Fog | LFA | opengameart.org/users/lfa |

### Act 3 audio
| Asset | Creator | Source | Used for |
|---|---|---|---|
| Evilhome BGM | ikimiuki | ikimiuki.itch.io | Lab discovery and final boss music |
| Boss Battle Theme | | opengameart.org/content/boss-battle-theme | Boss music |
| Good-Bye | | opengameart.org/content/good-bye | Credits music |
| Elemental Projectile | dklon | opengameart.org/users/dklon | Elemental projectile |
| Missile Launcher | dklon | opengameart.org/users/dklon | Missile launch |
| Final Boss Death | OptimusGnu | opengameart.org/users/optimusgnu | Boss death |
| Fire Cannon | Thimras | opengameart.org/users/thimras | Fire cannon |
| Smoke | pyranostudios | opengameart.org/users/pyranostudios | Smoke |
| Laser | Kenney | kenney.nl | Laser |
| Explosion | Dragon-Studio | pixabay.com/users/dragon-studio-38165424 | Explosions |

All third party assets are used under their creators' licences.
