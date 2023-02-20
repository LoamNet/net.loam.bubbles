# Bubbles
A zen puzzle game about collecting as many bubbles with a line as possible. This game is free, and the source is available to view/explore. 

## Platforms:
- Web (doesn't always save progress in-browser)
- Android
- Windows

Play or download at https://loam.net/games/

---

## Casual Retrospective

This game is no longer in active development. This was a project started as a HTML5 version of the unlimited mode, and to create more interesting situations and challenges I added the main challenges mode. I worked on it for a tad, but it was put down in a broken-ish state back in 2019 or so and did effectively no work until this year. I picked it back up a month or two ago to turn it into a little puzzle game for people could enjoy, but decided not to rewrite it to see what working with my old decisions and could would be like.

As of November 2022, the only changes are bug fixes and more challenges or challenge tweaks.

### Successes:
Vaguely ordered larger to smaller

- Separation of Data and Visuals. The visuals for the bubbles, lines, etc, and all effects are separate from the data driving the underlying state of the game like the bubble locations and sizes. This is pretty nifty, and has prevented a lot of headaches since tweaking how the game plays doesn't impact how the game looks, and vice versa. Also made it much easier to test both parts individually.

- Having an editor for the game built into the game is nice for rapid iteration, a whole lot better than manually placing bubbles in a scene. The ability to create a level, play it, then immediately go back to editing it rapidly came in later than it should have been, but my ability to make levels was so much better that I was able to meaningfully double challenge count extremely fast.

- Putting the odd dev timeline aside, the project was actually nicely scoped all things considered and leaves more room for content later if I want. It's a compact project with defined goals: 25+ challenges with 3 things to consider: Small static bubbles, Large bubbles that split, and bubble movement. 

- Having embedded debug line drawing for what would happen when a user does a given action made it a lot easier to check math.

### Misses:
Vaguely ordered larger to smaller

- Putting all the UI in the gameplay scene. This is the biggest miss. it makes it near impossible to see anything and once you're a few menus in, it's hard to make out UI without toggling things in the scene when editing which can lead to issues once playing.

- Gameplay level runner and level runner for edited levels were separate scenes. This doesn't really make sense. It duplicated scene hierarchies and caused edge cases where I couldn't easily repro bugs without playing through puzzles directly. In a combined fix, I could could have reduced a lot of headache by making it a separate scene like I should have the UI. Only reason this wasn't the worst issue was because of the simplicity of the game.

These first two both stemmed from using the same pattern as the jam this was based on, and the false thought that to make a game that felt like it was in one cohesive space it literally had to be in the same space. This made my life hard when I came back to finish things, and in hindsight, not a great move.

- Not making buttons based on prefabs. The button internals were prefabs, but since a lot of the logic and masking is done at the level of the button itself, this kept me from improving the UI more.

- Using a custom data format. None of the data structures I used would have been a problem in JSON, and I uncovered a number of serialization bugs that wouldn't have existed if I'd used even the existing unity JSON solution.

- Event solution was pretty lacking. It was a file full of System.Action entries, which isn't necessarily the worst, but it had to be directly referenced and in practice it was a huge pain the moment multiple scenes were involved. In the future, a propre cross-scene event or message system would be better.

- Wrapping Vector2 in a custom DataPoint class. I'd guess the intent was to give more space for debugging and to encourage splitting visuals and data - but in practice I couldn't take advantage of it, the decision didn't really help enforce separation, and it was an unnecessary pain when going between types.

- A focus on struct data types when not needed. In cases where data only lasts a bit and there's no harm in having a reference, it's really just a perf hit in unity. And when there's a list or array in the struct? That's just asking for headaches. I'm surprised this wasn't a bigger issue.

- Not using Canvas Groups. Not that it would have mattered much given how the UI was configured, but they remove potential timing issues with setting active and would have let me do better fades and transitions with ease.
