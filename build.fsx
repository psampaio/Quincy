// include Fake lib
#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Paket
open Fake.Testing

Restore

// Properties
let artifactsDir = "./artifacts/"
let buildDir = "./artifacts/build/"
let testDir = "./artifacts/tests/"
let solutionFile = "Quincy.sln"

MSBuildDefaults <-{
    MSBuildDefaults with
        Verbosity = Some (Quiet)
}

// Targets
Target "Clean" (fun _ ->
    CleanDirs [artifactsDir]
)

Target "BuildApp" (fun _ ->
    !! "src/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "BuildTests" (fun _ ->
    !! "test/**/*.csproj"
      |> MSBuildRelease testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "RunTests" (fun _ ->
    !! (testDir @@ "*.Tests.dll")
      |> xUnit2 (fun p ->
        { p with
            HtmlOutputPath = Some (testDir @@ "html");
            ToolPath = @"packages/tests/xunit.runner.console/tools/xunit.console.exe"
          })
)

// Dependencies
"Clean"
  ==> "BuildApp"
  ==> "BuildTests"
  ==> "RunTests"

// start build
RunTargetOrDefault "RunTests"