# Save and Load Write Up by Aleksie Sanchez

While designing the storage system I considered a couple of different ways to store important player data as well as how feasable it would be to implement and look at if anything went wrong with it

With these as my main criteria I first started with Unity's PlayerPrefs
Some of the pros of PlayerPrefs is that it's already inside Unity and wouldn't require much additional work to implement
But the issue is that it only supports ints float and strings which is not enough for what I have in mind or if I decide I need to save an object and not just smaller things
I could see myself saving Dice objects which are modified and would be hard to create into a string whilst still making it easy to understand
I don't really have an issue with the data being stored unencrypted as I would like to let my players modify their save if they would like to play around with more technical aspects of my game

---
Following that I looked into more complex ways to store data, particularly by using Binary Serialization 
I watched the following video https://www.youtube.com/watch?v=XOjd_qU2Ido which explained a majority of the implementation, how it works, and its pros and cons
From this I learned that the player data would be harder to modify which is a con for me since I don't particularly want that in my game
Another con is that the files aren't really easy to read if I were to try debugging and issue with saving and loading since it's all in binary.
Though it does offer support for complex objects which is something I'm looking for in my save system

---
What I believe to be the best option is JSON files which follows a similar approach in terms of implementation as the Binary Serialization.
Some pros is that JSON stores everything in a way which is really readable for me and in theory the player to allow for easy debugging on my part and easy modification for both me and the player.
It'll store stuctured data in that same readable way which means I could pass objects into it and it would work well.
However the biggest issue I see with JSON is that while storing I would have to make sure I am both loading and saving correctly. Requires a little more management and set up on my part.
The idea is that it'll be easier to extend for later as I create more objects which need saving.