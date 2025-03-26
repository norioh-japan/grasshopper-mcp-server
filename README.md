# Grasshopper MCP Server

A Model Context Protocol (MCP) server implementation for Rhinoceros/Grasshopper integration, enabling AI models to interact with parametric design tools via natural language.

## Overview

This project implements an MCP server that connects Large Language Models (LLMs) to Rhinoceros and Grasshopper functionality, allowing for:

- Creating and manipulating parametric designs through natural language
- Managing Grasshopper definitions and components
- Executing design solutions and retrieving results
- Connecting AI systems to powerful computational design tools

## Architecture

The system consists of two main components:

1. **MCP Server** - A .NET Core application that implements the Model Context Protocol
2. **Grasshopper Plugin** - A component that runs within Grasshopper and communicates with the MCP server

```
[LLM] <--HTTP/WebSocket--> [MCP Server] <--API--> [Grasshopper Plugin] <--> [Rhinoceros]
```

## Requirements

- Rhinoceros 7 or higher
- .NET 6.0 SDK or higher
- Visual Studio 2022 or VS Code with C# extensions

## Getting Started

See the Installation Guide in the docs directory for detailed setup instructions.

## Development

See the Developer Guide in the docs directory for information on building and extending the server.

## License

This project is licensed under the MIT License.
