This Event System is very simple, yet very powerful!
It currently includes only the core features:
- GameEvent Scriptable Object Class
- GameEventListener Component to be attached to a Game Object in the scene

~ Setup:

1. Add the Game Event Listener component to any Game Object in the Hierarchy
	*You could have a GO for handling all Scene-wide Event calls but individual GOs can also listen for Events
2. Create a new GameEvent SO in the SO_Events folder
3. Drag the SO reference into the Game Event Listener and set the Response (you can add as many or as few as needed)
4. Call the Event from anywhere in the scene by using GameEvent.Raise()

It's that simple!

~ Notes:

Debugging is easy, as you can trace the steps (is the event being raised? If so, is the Response being called? etc.)
You could also use a Button and directly set the reference in OnClick() to test an Event.

The idea is that you can achieve a lot within the Editor alone, with as little code as possible.
To aid in achieving this goal, I plan to expand and refine this system over time, to further reduce setup and clutter.
As well, new features (that I deem useful to my games) will be added as and when I decide they are necessary.

Happy Developing!