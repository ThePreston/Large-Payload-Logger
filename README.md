# Large Payload Logger
Created to overcome the 200kb limitation of APIM Policy LogToEventHub and also to overcome the 1MB limitation of EventHubs.

## Methods

### ClaimCheckLogger - saves content data to blob storage but saves context information to Event Hub
This overcomes the 200KB APIM policy limit but still uses Event Hubs to preserve existing functionality

### BlobLogger - saves content data to blob storage 
This overcomes both the 200KB APIM policy and the 1MB EventHub limit
