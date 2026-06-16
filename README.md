# Migration Tools

Editor tools for cleaning up missing scripts, broken references, and third-party Visual Scripting graphs when migrating Unity projects between platforms.

When a 3D platform shuts down or drops the features you built on, you are often left with a Unity project full of that platform's proprietary scripts. They throw errors, and a deployed build can load to a black screen because of them. These tools help you find what is broken, mark it so you can see where it was, and clear out the parts that only worked on the old platform, so you can rebuild without starting from scratch.

These tools started inside the Unreality3D Creator Dashboard. This is the same set, packaged on its own so you can use them in any Unity project without bringing in the whole template.

## What it does

- Replaces missing scripts with visible placeholder components, so you can see where a script used to be before you decide to remove it.
- Finds missing object references inside components that still exist, and helps you locate them for rewiring.
- Clears broken Visual Scripting graphs that contain another platform's node types, the kind that cause "could not deserialize" errors and builds that load to a black screen.
- Highlights every problem object right in the Scene view, color coded, so nothing is a blind operation.

## What it does not do

This is not a magic port. Proprietary hooks that a platform embedded throughout a project cannot all be untangled automatically. These tools clean up what is left and save you real time, but rebuilding the lost functionality is still your work. They get better as more projects come through them.

## Requirements

- Unity 6.
- Unity Visual Scripting (`com.unity.visualscripting`) is needed only for the two Visual Scripting cleanup tools. Projects coming from platforms that used visual scripting usually already have it. If it is not installed, those two tools show a note and the rest still work.

## Install

1. Download the latest `MigrationTools.unitypackage` from [Releases](https://github.com/unreality3d-platform/migration-tools/releases/latest).
2. In Unity: **Assets > Import Package > Custom Package**, choose the file, and import.
3. Open the tools from **Tools > Unreality3D > Migration Tools**.

The full walkthrough is on the [project page](https://unreality3d-platform.github.io/migration-tools/).

## How to use

Open **Tools > Unreality3D > Migration Tools** and start at the top:

1. Turn on **Highlight issues in the Scene view** and click **Scan and Select All Issues** to see what needs attention. Red is a missing script, yellow is a missing reference, blue is a placeholder. Nothing in this step changes your scene.
2. Work through the Missing Scripts, Missing References, and Visual Scripting sections. Each tool explains what it does, and every one can be undone with Ctrl+Z.

To clean Visual Scripting graphs from a platform other than Spatial, add that platform's namespace prefix to `ThirdPartyTokens` at the top of `AssetCleanupTools.cs`. It is the prefix shown in the red "Script Missing" nodes in the graph editor.

## Repository layout

- `Assets/Migration Tools/Runtime/`: the placeholder components that mark missing scripts and references.
- `Assets/Migration Tools/Editor/`: the cleanup tools, the scanner and Scene view highlight, and the tools window.
- `index.html`: the project page, served through GitHub Pages.

## License

MIT. See [LICENSE](LICENSE).
