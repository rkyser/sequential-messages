# Sequential Messages

This progam is intended to demonstrate an algorithm which:
1. Sorts out-of-order messages
2. Ignores duplicate messages

A component relying upon the `OrderedMessageReceiver` can expect that all 
messages dispatched are unique and sorted by the message's sequence ID.

# Running It

To run this app, clone this repo and run the following command.

```
dotnet run
```

