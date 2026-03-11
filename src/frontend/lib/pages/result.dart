import "package:flutter/material.dart";
import "package:pdfx/pdfx.dart";
import "package:frontend/styles/colors.dart";

class ResultArguments {
  final String pdfPath;

  ResultArguments(this.pdfPath);
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
    return Scaffold(
      backgroundColor: ThemeColors.offwhite,
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
                    icon: Icon(Icons.save, color: ThemeColors.offwhite),
                    onPressed: () => (),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: IconButton(
                    icon: Icon(Icons.arrow_back, color: ThemeColors.offwhite),
                    onPressed: () => Navigator.pop(context),
                  ),
                ),
              ],
            ),
          ),
          Expanded(
            child: Center(
              child: Container(
                width: 400,
                height: 400,
                child: PdfView(
                  controller: PdfController(
                    document: PdfDocument.openFile(args.pdfPath),
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
