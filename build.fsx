// include Fake lib
#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./artifacts"
let solutionFile = "Quincy.sln"

MSBuildDefaults <-{
    MSBuildDefaults with
        Verbosity = Some (Quiet)
}

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir]
)

Target "BuildApp" (fun _ ->
    !! solutionFile
     |> MSBuildRelease buildDir "Build"
     |> Log "AppBuild-Output: "
)

// Dependencies
"Clean"
  ==> "BuildApp"

// start build
RunTargetOrDefault "BuildApp"