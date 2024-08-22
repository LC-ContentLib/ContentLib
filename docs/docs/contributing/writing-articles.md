# Writing Articles

## About

We use [docfx](https://dotnet.github.io/docfx/) for ContentLib's documentation. These pages under the **Docs** section are fully handwritten, and can be found inside the [/docs/docs/](https://github.com/LC-ContentLib/ContentLib/tree/main/docs/docs) directory in the [ContentLib](https://github.com/LC-ContentLib/ContentLib) repository. The parent docs directory is for docfx configuration.

The [toc.yml](https://github.com/LC-ContentLib/ContentLib/blob/main/docs/docs/toc.yml) file inside the [/docs/docs/](https://github.com/LC-ContentLib/ContentLib/tree/main/docs/docs) directory is for configuring what articles go to the side panel on the left. You can locate each file from the `Edit this page` button at the bottom of each page.

## Adding New Articles

To contribute to ContentLib, you must create a fork of the repository. After you have a repository, create a new branch for your planned changes, for example `foobar-article`. You can then create a new article in the [/docs/docs/](https://github.com/LC-ContentLib/ContentLib/tree/main/docs/docs) directory on your branch.

Keep in mind that the directory where an article is placed will be a part of the URL. Therefore articles should be placed inside directories that match the name of the section of the article on the side panel.

After you've made your article, add it to the [/docs/docs/toc.yml](https://github.com/LC-ContentLib/ContentLib/blob/main/docs/docs/toc.yml) file under the appropriate section, and it'll appear on the left side panel.

Finally, you can open a pull request for your changes.

## Previewing Docs Site Locally

To see what your article actually looks like, make sure you have [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed, and then from the commandline you can install docfx:  
`dotnet tool update -g docfx`

After you have installed docfx, run the following command at the root of the ContentLib repository to run the website locally:  
`docfx .\docs\docfx.json --serve`

Then open the link you see on your console and the docs site should be up and running with your local changes. Notice that you need to restart the site each time you want to refresh your changes. This can be done by running the previous command again.

If you want to preview all the API docs pulled from the code and code documentation, you should build the whole project with the `Release` configuration before running the site locally to see the changes. From the commandline:  
`dotnet build -c Release`
