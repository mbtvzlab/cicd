{
  inputs.nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";

  outputs =
    { self, nixpkgs }:
    let
      forAllSystems = nixpkgs.lib.genAttrs nixpkgs.lib.systems.flakeExposed;
    in
    {
      devShells = forAllSystems (
        system:
        let
          pkgs = import nixpkgs {
            inherit system;
          };
        in
        {
          default = pkgs.mkShell {
            DOTNET_ROOT = "${pkgs.dotnet-sdk_10}/share/dotnet";
            buildInputs = with pkgs; [
              dotnet-sdk_10
              dotnet-ef
              omnisharp-roslyn

              # AI use mandated by course 🥀🥀🥀
              opencode
            ];
          };
        }
      );
    };
}
