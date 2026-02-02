using System;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;

namespace first_shader;

class Program
{
    private static IWindow? window;
    private static GL? gl;
    private static uint vao;
    private static uint vbo;
    private static uint shaderProgram;

    static void Main(string[] args)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "First Shader - OpenGL";
        
        window = Window.Create(options);
        
        window.Load += OnLoad;
        window.Render += OnRender;
        window.Closing += OnClose;
        
        window.Run();
    }

    private static unsafe void OnLoad()
    {
        gl = GL.GetApi(window);
        
        // Vertex data: position (x,y,z) and color (r,g,b)
        float[] vertices = {
            // positions        // colors
            -0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  // bottom left (red)
             0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  // bottom right (green)
             0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f   // top (blue)
        };
        
        // Create VAO and VBO
        vao = gl.GenVertexArray();
        vbo = gl.GenBuffer();
        
        gl.BindVertexArray(vao);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        
        unsafe
        {
            fixed (float* v = &vertices[0])
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), v, BufferUsageARB.StaticDraw);
            }
        }
        
        // Position attribute
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)0);
        gl.EnableVertexAttribArray(0);
        
        // Color attribute
        gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)(3 * sizeof(float)));
        gl.EnableVertexAttribArray(1);
        
        // Load and compile shaders
        shaderProgram = CreateShaderProgram("vertex_shader.glsl", "fragment_shader.glsl");
        
        Console.WriteLine("Shader loaded successfully! Rendering a colorful triangle.");
    }

    private static void OnRender(double deltaTime)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit);
        
        gl.UseProgram(shaderProgram);
        gl.BindVertexArray(vao);
        gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    private static void OnClose()
    {
        gl.DeleteBuffer(vbo);
        gl.DeleteVertexArray(vao);
        gl.DeleteProgram(shaderProgram);
    }

    private static uint CreateShaderProgram(string vertexPath, string fragmentPath)
    {
        string vertexCode = File.ReadAllText(vertexPath);
        string fragmentCode = File.ReadAllText(fragmentPath);
        
        uint vertexShader = CompileShader(ShaderType.VertexShader, vertexCode);
        uint fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentCode);
        
        uint program = gl.CreateProgram();
        gl.AttachShader(program, vertexShader);
        gl.AttachShader(program, fragmentShader);
        gl.LinkProgram(program);
        
        gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int success);
        if (success == 0)
        {
            string info = gl.GetProgramInfoLog(program);
            throw new Exception($"Shader program linking failed: {info}");
        }
        
        gl.DeleteShader(vertexShader);
        gl.DeleteShader(fragmentShader);
        
        return program;
    }

    private static uint CompileShader(ShaderType type, string source)
    {
        uint shader = gl.CreateShader(type);
        gl.ShaderSource(shader, source);
        gl.CompileShader(shader);
        
        gl.GetShader(shader, ShaderParameterName.CompileStatus, out int success);
        if (success == 0)
        {
            string info = gl.GetShaderInfoLog(shader);
            throw new Exception($"{type} compilation failed: {info}");
        }
        
        return shader;
    }
}