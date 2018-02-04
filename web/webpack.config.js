const path = require('path');
var webpack = require('webpack');

const HtmlWebpackPlugin = require('html-webpack-plugin');
const ExtractTextPlugin = require("extract-text-webpack-plugin");

const HtmlWebpackPluginConfig = new HtmlWebpackPlugin({
  template: './src/index.html',
  filename: 'index.html',
  inject: 'body'
});
const ExtractTextPluginConfig = new ExtractTextPlugin(
    "styles.css"
);
const WatchIgnorePluginConfig = new webpack.WatchIgnorePlugin([
    /css\.d\.ts$/
]);


module.exports = {
    entry: "./src/index.tsx",
    output: {
        filename: "bundle.js",
        path: __dirname + "/dist"
    },

    // Enable sourcemaps for debugging webpack's output.
    devtool: "source-map",

    resolve: {
        // Add '.ts' and '.tsx' as resolvable extensions.
        extensions: [".ts", ".tsx", ".js", ".json"]
    },

    module: {
        rules: [
            // All files with a '.ts' or '.tsx' extension will be handled by 'awesome-typescript-loader'.
            {
                test: /\.tsx?$/,
                loader: "awesome-typescript-loader",
                exclude: /node_modules/
            },

            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
            {
                test: /\.js$/,
                enforce: "pre",
                loader: "source-map-loader",
                exclude: /node_modules/
            },

            {
                test:/\.css$/, 
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: 'typings-for-css-modules-loader?modules&namedExport&camelCase'
                })
            },

            // { test: /\.js$/, loader: 'babel-loader', exclude: /node_modules/ },
      
            // { test: /\.jsx$/, loader: 'babel-loader', exclude: /node_modules/ }   
        ]
    },

    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
    // externals: {
    //     "react": "React",
    //     "react-dom": "ReactDOM"
    // },

    plugins: [
        // Injects .js and .css into html.
        HtmlWebpackPluginConfig,
        // Saves .css to files.
        ExtractTextPluginConfig,
        // Ignores .d.ts files for typescript
        //WatchIgnorePluginConfig
    ]
};