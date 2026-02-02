# First Shader Project

A basic OpenGL shader project using C# and Silk.NET that renders a colorful triangle.

## Preview

![Shader Output](/images/first_pic.png)

## Features

- OpenGL 4.1 rendering
- Vertex and fragment shaders
- Color interpolation across triangle vertices

## Prerequisites

- .NET 10.0 SDK
- JetBrains Rider (or any C# IDE)

## Project Structure

```
shader/
├── Program.cs              # Main application code
├── vertex_shader.glsl      # Vertex shader
├── fragment_shader.glsl    # Fragment shader
└── shader.csproj          # Project configuration
```

## Dependencies

- Silk.NET.OpenGL (v2.21.0)
- Silk.NET.Windowing (v2.21.0)
- Silk.NET.Maths (v2.21.0)

## How to Run

1. Open the project in Rider
2. Build the project (Cmd+Shift+F9)
3. Run the project (Shift+F10)

A window will appear displaying a triangle with interpolated colors (red, green, and blue at each vertex).

## Shaders

### Vertex Shader (`vertex_shader.glsl`)
Transforms vertex positions and passes color data to the fragment shader.

### Fragment Shader (`fragment_shader.glsl`)
Receives interpolated color values and outputs the final pixel color.

## Output

The program renders a triangle with:
- Red vertex at bottom-left
- Green vertex at bottom-right
- Blue vertex at top
- Smooth color interpolation across the surface

## Notes

- This project uses OpenGL 4.1 for macOS compatibility
- The shaders are loaded from separate `.glsl` files
- Unsafe code is enabled for direct memory operations with OpenGL
