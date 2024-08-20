# Large Payload Logger
Created to overcome the 200kb limitation of APIM Policy LogToEventHub and also to overcome the 1MB limitation of EventHubs. Works great, easy to deploy

## Architecture
![LargePayloadLog](https://github.com/ThePreston/Large-Payload-Logger/assets/84995595/8736b978-c08e-48cf-b0b9-20d190a4add6)



## Methods

### ClaimCheckLogger - Saves content data to blob storage but saves context information to Event Hub
This overcomes the 200KB APIM policy limit but still uses Event Hubs to preserve existing functionality

### BlobLogger - Saves content data to blob storage 
This overcomes both the 200KB APIM policy and the 1MB EventHub limit

## APIM Policy Fragments
Inbound and Outbound snippets
