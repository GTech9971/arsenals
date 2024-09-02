# MAUI

## 環境

- macOS Sonoma 14.6.1 M1
- VSCode
  - .NET MAUI

## ホットリロード

- 拡張機能`.NET MAUI`をインストールしていれば、
    デバックで`.NET MAUI`を選んで起動すれば自動でホットリロードになる

- ルートディレクトリのxamlしか見ていないため、そこ以外にページを作成した場合、`.csproj`に以下を追記してホットリロード対象に指定させる必要がある
  - ワイルドカード指定でも、ファイル名全指定でも動く

```xml
 <ItemGroup>
  <MauiXaml Update="Views\*.xaml.cs">
   <Generator>MSBuild:Compile</Generator>
  </MauiXaml>
 </ItemGroup>
```
