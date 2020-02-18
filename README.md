# What is this

A C# wrapper for the Firebase Javascript sdk for unity's WebGL platform support.


# Why

Because the official [Unity Firebase SDK](https://firebase.google.com/docs/unity/setup) only supports Desktop, Android and iOS platforms.

This repository wraps the [Firebase Web SDK](https://firebase.google.com/docs/web/setup) with C# into DLLs usable by unity's runtime WebGL builds, allowing for a full advantage of all the functionality of the sdk while maintaining the same code structure of the unity's sdk so you're still able to build for other platforms without having to delete the WebGL DLLs.

## How to use
Build the DLLs of the services you want to use in your project, (Utilities and Firebase.App) must be build with any service you import to the unity project.

Make sure the WebGL DLLs build target is set to WebGL only, so unity editor doesn't try to compile them.
The build comes with **.jslib** files these are unity's javascript files and are automatically copied to the build folder.
