const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin') // html-webpack-plugin add as plugins to module.exports then dist can be deleted
const Dotenv = require('dotenv-webpack')

module.exports = {
    mode: 'development',
    entry: {
        main: path.resolve(__dirname, 'src/index.js'),
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: '[name].[contenthash].js', // name of entry as an output generated js
        clean: true,
        assetModuleFilename: '[name][ext]',
        publicPath: "/"
    },
    devtool: 'source-map',
    devServer: {
        static: {
            directory: path.resolve(__dirname, 'dist')
        },
        port: 3000,
        open: true, // open browser automatically if true
        hot: true, // hot reloading
        compress: true, // gzip compression
        historyApiFallback: true // serve on every url index.html if true
    },
    module: {
        rules: [
            {
                // any file with extension scss
                test: /\.(sass|scss|css)$/,
                use: [
                    'style-loader', 
                    'css-loader',
                    'sass-loader'
                ]
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: 'asset/resource',
            }
        ]
    },
    plugins: [
        new HtmlWebpackPlugin({
            title: 'MyOrders-App',
            filename: 'index.html',
            template: 'src/template.html'
        }),
        new Dotenv()
    ]
}