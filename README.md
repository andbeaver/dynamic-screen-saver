# Dynamic Screen Saver Simulation

A C# Windows Forms application that simulates an interactive screen saver with multiple bouncing shapes, collision detection, and dynamic visual effects.

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue)
![C#](https://img.shields.io/badge/C%23-7.3-green)
![License](https://img.shields.io/badge/license-MIT-blue)

## 📋 Overview

This project demonstrates object-oriented programming principles through a dynamic screen saver simulation. Multiple shapes move across the screen, bounce off walls, and interact with each other through collision detection. Each shape type exhibits unique behaviors and visual effects.

## ✨ Features

### Core Functionality
- **Multiple Shape Types**: Circle, Rectangle, Triangle, Irregular Polygon, and Picture Box shapes
- **Polymorphic Design**: All shapes inherit from a base `Shape` class
- **Collision Detection**: Shapes detect and respond to collisions with each other
- **Wall Bouncing**: Shapes bounce off the window boundaries
- **Smooth Animation**: Runs at ~60 FPS for fluid movement
- **Runtime Shape Creation**: Click anywhere to add random shapes during execution

### Visual Effects
- **Dynamic Color Changes**: Shapes change colors upon collision
- **Gradient Toggle**: Rectangles toggle gradient fills when colliding
- **Pulse Effect**: Triangles pulse in size after collisions
- **Image Flipping**: Picture box shapes flip horizontally on impact
- **Shape Morphing**: Irregular polygons regenerate their geometry on collision
- **Smooth Rendering**: Double-buffered graphics to reduce flickering

### Interactive Features
- **Mouse Click Interaction**: Click to spawn random shapes at cursor position
- **Responsive Resizing**: Shapes adapt to window size changes
- **Resource Management**: Proper cleanup of resources on application close

## 🛠️ Technologies Used

- **Language**: C# 7.3
- **Framework**: .NET Framework 4.8
- **UI Framework**: Windows Forms
- **Graphics**: System.Drawing (GDI+)

## 🏗️ Project Structure

```
Adv_Assignment2/
├── Shapes/                          # Shape classes organized in folder
│   ├── Shape.cs                     # Abstract base class
│   ├── Circle.cs                    # Circle implementation with color timer
│   ├── Rectangle.cs                 # Rectangle with gradient effects
│   ├── Triangle.cs                  # Triangle with pulse animation
│   ├── PictureBoxShape.cs           # Image-based shape with flipping
│   └── IrregularPolygon.cs          # Random polygon with regeneration
├── Form1.cs                         # Main form with animation logic
├── Form1.Designer.cs                # Form designer code
├── Program.cs                       # Application entry point
├── Properties/                      # Project properties and resources
└── Resources/                       # Embedded images (football_img.jpg)
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2017 or later
- .NET Framework 4.8 SDK
- Windows OS

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone https://github.com/andbeaver/dynamic-screen-saver.git
   cd dynamic-screen-saver
   ```

2. **Open in Visual Studio**
   - Open `Adv_Assignment2.sln` in Visual Studio
   - Restore NuGet packages (if any)

3. **Build and Run**
   - Press `F5` or click the "Start" button
   - The application window will launch with animated shapes

### Usage

- **Watch the Animation**: Shapes automatically move and bounce around
- **Add Shapes**: Click anywhere in the window to spawn a random shape
- **Resize Window**: Drag the window borders - shapes will adjust and stay within bounds
- **Close Application**: Click the close button or press `Alt+F4`

## 🎓 Object-Oriented Principles Demonstrated

### Inheritance
- All shapes inherit from the abstract `Shape` base class
- Common properties (X, Y, VelX, VelY, Color, Width, Height) in base class
- Each shape implements specific drawing and collision logic

### Polymorphism
- **Late Binding**: `shapes.Move()` and `shapes.Draw()` called on base type
- **Runtime Type Checking**: `if (shape is Rectangle rect)` for type-specific behavior
- **Method Overriding**: Each shape overrides `Draw()`, `Move()`, and `CollidesWith()`

### Encapsulation
- Shape properties encapsulated with getters/setters
- Internal state managed within each shape class
- Clean separation of concerns (shapes, form, animation)

### Abstraction
- Abstract `Shape` class defines the contract for all shapes
- Concrete implementations provide specific behaviors
- Common interface for collision detection and rendering

## 🎨 Shape Behaviors

| Shape | Visual Effect on Collision | Special Feature |
|-------|---------------------------|-----------------|
| **Circle** | Color change | Color timer animation |
| **Rectangle** | Color change + Gradient toggle | Solid/gradient fill switching |
| **Triangle** | Color change + Pulse effect | Size pulsing animation |
| **PictureBoxShape** | Image flip | Horizontal image mirroring |
| **IrregularPolygon** | Color change + Regenerate | Random point regeneration |

## 🔧 Configuration

### Animation Speed
Modify `animationTimer.Interval` in `Form1.cs` (line 48):
```csharp
animationTimer.Interval = 16; // ~60 FPS (default)
```

### Shape Speed Range
Adjust `RandomSpeed()` method (line 104):
```csharp
random.Next(2, 6) // Min: 2, Max: 5 pixels per frame
```

### Initial Shape Count
Add/remove shape initialization calls in `InitializeApplication()` (lines 37-42)

## 📸 Screenshots

<!-- Add screenshots here -->
```
[Screenshot 1: Application in action]
[Screenshot 2: Multiple shapes colliding]
[Screenshot 3: Different shape types]
```

## 🐛 Known Issues

- Shapes may occasionally overlap when window is rapidly resized
- High shape counts (>50) may impact performance on older hardware

## 🔮 Future Enhancements

- [ ] Add shape removal on right-click
- [ ] Implement gravity mode
- [ ] Add shape size controls
- [ ] Include sound effects on collisions
- [ ] Support for custom images
- [ ] Save/load shape configurations
- [ ] Performance optimization for large shape counts
- [ ] Add shape trails/motion blur effect

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 👤 Author

**Andrew Beaver**
- GitHub: [@andbeaver](https://github.com/andbeaver)
- Repository: [dynamic-screen-saver](https://github.com/andbeaver/dynamic-screen-saver)

## 🙏 Acknowledgments

- Built as part of an Advanced Topics in Programming course
- Demonstrates practical application of OOP concepts in C#
- Uses Windows Forms for rapid GUI development

---

⭐ **Star this repository if you find it helpful!**
