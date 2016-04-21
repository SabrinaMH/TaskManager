# TaskManager
Keep track of tasks and their prioritization.

Requirements to run solution:
- GetEventStore running on port 2113 for http and 1113 for tcp
- Couchbase running on port 8091 with the following buckets created (including primary index): 
  - ProjectTreeNode
  - Test_ProjectTreeNode (if you want to run the test suite)
