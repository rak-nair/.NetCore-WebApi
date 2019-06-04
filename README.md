# LimespotAssignment

The assignment itself  - 
 
The Transformers.  The Autobots and Decepticons are a race of mechanical beings at war on the planet Cybertron. They have been battling it out for centuries but have decided that the only real way to end their war is through a computerized simulation. 
 
(You might naively think that this is an unrealistic way for a millenia old race to resolve their galactic civil war, but there’s actually previous precedent.) 
About the Transformers 
 
Every Transformer has the following properties (an example of which is handily denoted here): 
 
●	Allegiance (A Transformer can only be an Autobot or a Decepticon) 
●	Strength (1 through 10) 
●	Intelligence (1 through 10) 
●	Speed (1 through 10) 
●	Endurance (1 through 10) 
●	Rank (1 through 10) 
●	Courage (1 through 10) 
●	Firepower (1 through 10) 
●	Skill (1 through 10) 
 
A Transformer’s “overall rating” is the sum of every property other than Allegiance. 
 
Mission details 
The world needs you to build a simulation that allows for the following actions: 
 
1.	Adding a new Transformer 
2.	Retrieving the details of a registered Transformer 
3.	Updating a Transformer 
4.	Removing a registered Transformer 
5.	Getting a list of all the registered Autobots, sorted alphabetically 
6.	Getting a list of all the registered Decepticons, sorted alphabetically 
7.	Given a Transformer, return the overall score (see “technical constraints” though, below) 
8.	Running the war between the list of Transformers currently registered and returning a list of the victors 
 
 
Technical notes 
●	This simulation should be built using either the Microsoft Web API or ASP .NET Core (your choice), with HTTP endpoints corresponding to Mission Details 1 through 8. 
●	SQL Server should be used as the backing store. You’re welcome to use an ORM of your choice for most mappings. 
●	However, endpoint #7 should be implemented as a stored procedure; whether you want to call that stored proc through the ORM is up to you. We know, weird request =) 
○ You can still have a method to calculate an individual transformers overall score in the C# code (you might find it helpful for other endpoints...;) ), just that endpoint #7 should derive its value from the stored proc. 
●	How you handle the endpoint naming scheme is up to you, however the Transformers have a strong preference for REST endpoints. 
●	How you handle the content of the responses are up to you, but assume that JSON is the expected response from at least 5 through 8.  
●	The API can be open - it does not need to have authentication implemented. 
●	As the Transformers are primarily machines, a UI is not required.  
●	You are welcome to use whatever open-source packages you like in your submission - just highlight what you used (and why) when you submit.  
 
Simulating the war 
The war is a sequence of battles between the registered Autobots and Decepticons. The 
Autobots and Decepticons are sorted by rank (highest rank first), and then battles run in pairs (e.g. Autobot 1 vs. Decepticon 1, Autobot 2 vs Decepticon 2, etc.) until there are no more pairs left to evaluate. 
 
Rules of battle (in order of preference): 
 
●	If a Transformer is named Optimus or Predaking, the battle ends automatically with them as the victor 
●	If Transformer A exceeds Transformer B in strength by 3 or more and Transformer B has less than 5 courage, the battle is won by Transformer A (Transformer B ran away) 
●	If Transformer A’s skill rating exceeds Transformer B's rating by 5 or more, Transformer A wins the fight. 
●	Otherwise, the victor is whomever of Transformer A and B has the higher overall rating. (You can determine what to do in the event of a tie between two robots). 
 
The end result of a battle should be a list of the survivors/victors on each side.  
 
The sides do not have to be equal numbers to have a battle.  For example, 5 Autobots vs. 3 Decepticons should only have 3 battles, with the other 2 Decepticons being counted as survivors.  
 
Special note: if Optimus is on one team and Predaking is on another team, the battle ends **with no victors on either side. ** 
