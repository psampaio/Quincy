// include Fake lib
#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Paket
open Fake.Testing

Restore

// Properties
let artifactsDir = "./artifacts/"
let buildDir = artifactsDir @@ "build/"
let testDir = artifactsDir @@ "tests/"

MSBuildDefaults <-{
    MSBuildDefaults with
        Verbosity = Some (Minimal)
}

// Targets
Target "Clean" (fun _ ->
    CleanDirs [artifactsDir]
)

Target "UpdateAssemblyInfo" (fun _ ->
    if environVar "APPVEYOR" = "True" then Shell.Exec("gitversion","/l console /output buildserver /updateassemblyinfo" ) |> ignore
)

Target "BuildApp" (fun _ ->
    !! "src/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> ignore
)

Target "BuildTests" (fun _ ->
    !! "test/**/*.csproj"
      |> MSBuildRelease testDir "Build"
      |> ignore
)

Target "RunTests" (fun _ ->
    !! (testDir @@ "*.Tests.dll")
      |> xUnit2 (fun p ->
        { p with
            HtmlOutputPath = Some (testDir @@ "xunit.html");
            ToolPath = @"packages/tests/xunit.runner.console/tools/xunit.console.exe"
          })
)

Target "Package" (fun _ ->
    Pack (fun p ->
      { p with
          OutputPath = artifactsDir
      })
)

// Dependencies
"Clean"
  ==> "UpdateAssemblyInfo"
  ==> "BuildApp"
  ==> "BuildTests"
  ==> "RunTests"
  ==> "Package"

// start build
RunTargetOrDefault "Package"