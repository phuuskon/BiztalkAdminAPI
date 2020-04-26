# BiztalkAdminAPI

This is a complementary API for BizTalk Management API shipped with BizTalk.

## Operations 

### BizTalkStatus

With BizTalkStatus you can get an overview of BizTalk Server current state.
Returned fields:
- HostInstancesRunning: Number of host instances in running state
- HostInstancesNotRunning: Number of host instance in state other that running
- ReceiveLocationsEnabled: Number of receive locations enabled
- ReceiveLocationsDisabled: Number of receive locations disabled
- SuspendedInstancesCount: Number of suspended service instances
- RunningInstancesCount: Number of running service instances

### HostInstance

With HostInstance operations you can list and start/stop host instances running in BizTalk.
Returned fields:
- Caption
- ClusterInstanceType
- ConfigurationState
- Description
- HostName
- HostType
- InstallDate
- IsDisabled
- Logon
- MgmtDbNameOverride
- MgmtDbServerOverride
- Name
- NTGroupName
- RunningServer
- ServiceState
- Status
- UniqueID

### ReceiveLocation

With ReceiveLocation you can get list of receive locations in BizTalk. 
Returned fields:
- ApplicationName: Application containing receive location
- ReceivePortName: Receive port name of the receive location
- ReceiveLocationName: Name of the receive location
- Disabled: true if receive location disabled, false if enabled
- LastMessageReceivedDateTime: Latest datetime when receivelocation received message, null if never

### SendPort

With SendPort you can get list of sendports in BizTalk. 
Returned fields:
- ApplicationName: Application containing sendport
- SendPortName: Sendport name
- PortStatus: possible statuses of port None, Unenlisted, Stopped, Started
- LastMessageSentDateTime: Latest datetime when sendport sent message, null if never

### Orchestration

With Orchestration you can get list of orchestrations in BizTalk. 
Returned fields:
- ApplicationName: Application containing orchestration
- OrchestrationName: Full name of the orchestration
- OrchestrationStatus: possible statuses of orchestration None, Unenlisted, Stopped, Started
- LastStartDateTime: Latest datetime when orchestration started, null if never

## Installation

Publish to IIS of the BizTalk server and run in app pool running with credentials that are in  Biztalk Server Administrator group.

## Swagger documentation

Go to http://<your_bts>/BizTalkAdminAPI/Swagger