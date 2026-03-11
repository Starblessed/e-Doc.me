import "dart:io";

import "package:flutter/material.dart";
import "package:pdfx/pdfx.dart";
import "package:frontend/styles/colors.dart";

class ResultArguments {
  final String imagePath;

  ResultArguments(this.imagePath);
}

class ResultPage extends StatefulWidget {
  const ResultPage({super.key});

  @override
  State<ResultPage> createState() => _ResultPageState();
}

class _ResultPageState extends State<ResultPage> {
  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context)!.settings.arguments as ResultArguments;

    Image image = Image.file(File(args.imagePath));
    return Scaffold(
      backgroundColor: Colors.grey[700],
      body: Row(
        children: [
          Container(
            width: 80,
            color: ThemeColors.aquaForest,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: IconButton(
                    icon: Icon(Icons.save, color: ThemeColors.gold),
                    onPressed: () => (),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: IconButton(
                    icon: Icon(Icons.arrow_back, color: ThemeColors.gold),
                    onPressed: () => Navigator.pop(context),
                  ),
                ),
              ],
            ),
          ),
          Expanded(
            child: InteractiveViewer(
              child: Center(
                child: Container(
                  height: 400,
                  decoration: BoxDecoration(border: Border.all()),
                  child: image,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
