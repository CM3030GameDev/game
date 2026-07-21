# Game Project Repository
---
## Git + GitHub Desktop + Unity - Quick Guide
## First Time Setup
1. Download & install [GitHub Desktop](https://desktop.github.com/)
2. Sign in with your GitHub account
3. **Clone the repo**: File → Clone Repository → paste the repo URL → choose a local folder

---

## Rules
- Don't work directly on `main`**
- Always work on **your own branch**
- **Pull before you start working** every session

---

## Daily Workflow

### 1. Pull latest changes
In GitHub Desktop:
> **Fetch origin** → then **Pull** (top bar)

### 2. Switch to / create your branch
> Branch → New Branch → name it something like `yourname-feature`  
> (e.g. `username-player-movement`)

### 3. Make your changes in Unity, then save the scene

### 4. Commit your changes
- GitHub Desktop will show all changed files on the left
- Write a short **commit message** (e.g. `Add player movement script`)
- Click **Commit to your-branch-name**

### 5. Push your branch
> Click **Push origin** (top bar)

### 6. Open a Pull Request (PR) when done
> GitHub Desktop will prompt you - or go to GitHub.com → your branch → **Compare & pull request**  
> Tag a groupmate to review before merging into `main`

---

## Unity-Specific Tips

### .gitignore
Make sure your repo has a Unity `.gitignore` (GitHub offers this when creating the repo).  
It excludes `Library/`, `Temp/`, `Logs/` - these should **never** be committed.

### Avoid merge conflicts
- **One person owns one scene at a time** - don't edit the same `.unity` file simultaneously
- Prefer editing **separate scripts/prefabs** over the same scene
- If you must share a scene, communicate in the group chat before editing

### If you get a merge conflict on a scene file
- It's messy - Unity scene files are not human-readable
- Best fix: one person keeps their version, the other **manually re-applies** their changes
- Prevent this by splitting work into prefabs and separate scenes where possible
