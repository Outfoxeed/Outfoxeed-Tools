//
// MIT License Copyright(c) 2025 Aiden Nathan, https://github.com/Agent40infinity/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

/// <summary>
/// Lightweight Attribute for string that allows Scene referencing in the Inspector.
/// Only used on strings and data collections that contain strings (Arrays, Lists, Collections)
/// 
/// Uses a property drawer to replace the default string drawer and replaces it with an object field for SceneAssets.
/// Additionally, when a SceneAttribute is not null, you can add & remove the scene from the build settings with a button.
/// 
/// Works by extracting the scenePath from the selected SceneAsset and can be used during runtime (SceneManager).
/// 
/// If the SceneAsset is moved, it'll keep track and replace the string automatically.
/// If the SceneAsset is renamed, it'll lose reference as the path and name of the asset are no longer related.
/// </summary>
public class SceneAttribute : UnityEngine.PropertyAttribute { }