# Quests

Create your own RPG-like quests with this library built on .NET Standard.

## Installation

This library targets .NET Standard 2.0 and .NET Standard 2.1. You can install this package over NuGet.
```
PM> Install-Package Micky5991.Quests
```

## Features

- Behavior-Tree inspired quest nesting.
- Sleeping, Active, Success and Failure states for quest nodes.
- Event-based Task completion
- Observe changes to quest properties using observer pattern.
- Full integration into Microsoft.Extensions.DependencyInjection.Abstractions 3.1+

## Requirements

- .NET Standard 2.0 / 2.1 compatible application
- Dependency Injection Container compatible with `Microsoft.Extensions.DependencyInjection.Abstractions`

### Node-Types

This library provides different node types out of the box. But you can inherit from already existing abstract types to create your own composite or task nodes.

- **Quest Root**\
   Parent node that holds context information and a blackboard, accessible by all child nodes. This node will be
   registered to the DI Container.
- **Quest Task**\
   Final tree leaf that listens to events and marks itself as a certain status to signal other parts of your application
   if a task has been completed. These nodes will hold actual actions the user has to do in order to progress through the tree.
- **Condition Task**\
   Final tree leaf that can be used as a "neutral" task that should not be failed. For example: "Stay alive during this attack." It will be marked as successful, but can be transitioned into a failed state.
- **Composite: Sequence**\
   Chain quest tasks or other composite nodes one after another to accomplish some kind of dependency between different tasks. It tries to finish every subtask.
- **Composite: Parallel**\
   Create a batch of tasks or other composite nodes which will be activated as soon as the parent activates. Here you can offer multiple quests to the target.
- **Composite: Any Success Sequence**\
   Create a try-and-error task chain that will succeed as soon as the first child node signals a successful state. Useful if you want to give your target repeated quests and fallback to another quest, if the first was not successful.

## Example quest structure

To provide you with some information about the possibilities this library offers, you can find an example Roleplay quest layout:

```
QUEST ROOT: Journey to save the princess
├─COMPOSITE SEQUENCE
│ ├─ TASK: Find your brother
│ ├─ COMPOSITE SEQUENCE
│ │ ├─ TASK: Enter the Bomb Battlefield.
│ │ ├─ TASK: Find the boss of this Level.
│ │ ├─ COMPOSITE PARALLEL
│ │ │ ├─ CONDITION TASK: Stay alive during the bossfight.
│ │ │ └─ TASK: Defeat the great bomb boss
│ │ └─ TASK: Leave the Bomb Battlefield.
│ ├─ COMPOSITE SEQUENCE
│ │ ├─ TASK: Enter the final world
│ │ └─ COMPOSITE PARALLEL
│ │   ├─ TASK: Defeat all enemies in this world
│ │   └─ COMPOSITE SEQUENCE
│ │     ├─ TASK: Find the last boss
│ │     ├─ COMPOSITE PARALLEL
│ │     │ ├─ CONDITION TASK: Stay alive during the bossfight.
│ │     │ └─ TASK: Defeat the final boss
│ │     └─ TASK: Find and free the princess
│ └─ TASK: Claim your reward
```

This is a quest structure which can be represented with this library.

## License

MIT License

Copyright (c) 2021 Francesco Paolocci

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
