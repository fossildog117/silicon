# Silicon

This repository consists of a variety of backend technologies developed for the COMP205P system engineering project.

# Chart Engine
This software receives a GET request from a REST request and generates a chart based on the GET request. The engine currently supports a variety of graphs including bar charts, line graphs and pie charts, and more graphs can be further added in the future. 

Data is retrieved on an Azure blob which is used to generate the graphs. We have done data retrieval against local servers which has shown to be significantly faster although this has not been included into the main Chart Engine due to the reason that the Set Engine now produces size mappings that have reduced chart generation speeds.




# Set Engine
The set engine is responsible for producing sets from any relational database. The engine contains a variety of other applications including the Navigator that is used to navigate a computer's file structure, DBConnector a general purpose database connector for connecting to MySQL databases and DecisionTree that is used to determine the number of sub-sets to generate for each set.

Additional features have been added to the set engine over the months including creation of subsets using a decision tree, generating set information to limit size of data being sent from the blob and set mappings where each set is mapped by a key-value mapping where the key is human readable. The set engine contains an additional set suite for unit testing.
