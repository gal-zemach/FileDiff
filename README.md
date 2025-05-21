# File Diff
### Compares files using the Myers Algorithm 

An application that prints the diff between two given files.
Prints human-readable instructions for transforming file1 into file2.
The app uses the Myers algorithm for computing the instructions.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Build Instructions](#build-instructions)
- [Usage](#usage)
- [File Structure](#file-structure)
- [References](#references)

---

## Prerequisites
- .NET 9.

---

## Build Instructions
- **For Windows (x64)**
```
dotnet publish -r win-x64 -c Release --self-contained true
```

- **For macOS (ARM)**
```
dotnet publish -r osx-arm64 -c Release --self-contained true
```

---

## Usage
```
FileDiff <file1> <file2>
```

---

## File Structure
```
FileDiff/
│
├─ Common/
│   └─ OffsetArray.cs   # An array that can be accessed using an offset index
│
├─ Diff/
│   └─ FileComparer.cs   # Returns grouped diff instructions with the desired output format
│   │
│   └─ Algorithm
│   │   └─ MyersComparer.cs   # Returns single-line diff instructions using the Myers algorithm
│   │
│   └─ Data
│       └─ Instruction.cs   # Base class for the instructions the app uses
│       └─ InsertInstruction.cs   # Instruction insterting new lines
│       └─ RemoveInstruction.cs   # Instruction for removing new lines
│
└─ Program.cs   # Application entry point
```

---

## References
- [Myers Algorithm Explanation](https://blog.jcoglan.com/2017/02/12/the-myers-diff-algorithm-part-1/) by James Coglan.
