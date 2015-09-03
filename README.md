# sqlrefactor
Little tool to ease the process of refactoring sql monsters. Provides a comparison between the original query and the refactored one as well as maintaining a history of the iterations.

Its a common scenario. You are faced with a massive three page SQL query that for some reason runs incredibly slowly. It's enough to put the fear of Beelzibub into any developer. 

The query needs to be refactored, but it is so complex that even the slightest wrong change could change the resultset drastically.

SQL Refactor is a simple app to make this process easier.

Paste the query into the Base Query tab.

Then paste it into the Iterations tab. And start refactoring. When you make a change, run the query. The app will compare the results of the new query with the base query and report any changes. If the results are the same keep on going until you are happy with the performance.

If the results are different, you know you have messed up the refactoring. Step back to the previous iteration and carry on from there.
