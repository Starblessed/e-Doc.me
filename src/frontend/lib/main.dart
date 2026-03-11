import 'package:flutter/material.dart';
import 'package:frontend/pages/result.dart';
import 'package:frontend/pages/loading.dart';
import 'package:frontend/services/scanner.dart';
import 'package:file_picker/file_picker.dart';
import 'package:frontend/styles/colors.dart';

void main() {
  runApp(
    MaterialApp(
      initialRoute: "/",
      routes: {
        "/": (ctx) => Home(),
        "/loading": (ctx) => LoadingPage(),
        "/result": (ctx) => ResultPage(),
      },
    ),
  );
}

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  String selectedFilePath = '';
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: ThemeColors.offwhite,
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            IconButton(
              onPressed: () async {
                try {
                  FilePickerResult? result = await FilePicker.platform
                      .pickFiles();

                  if (result == null) return;
                  final file = result.files.first;
                  final filePath = file.path;
                  if (filePath == null) return;
                  final task = await requestDocumentScan(filePath);

                  if (!context.mounted) return;
                  Navigator.pushNamed(context, "/loading", arguments: task);
                } catch (e) {
                  if (!context.mounted) return;

                  ScaffoldMessenger.of(
                    context,
                  ).showSnackBar(SnackBar(content: Text(e.toString())));
                }
              },
              icon: Icon(
                Icons.file_upload,
                color: ThemeColors.latte,
                size: 50.0,
              ),
            ),
            Text(
              "Upload an image to scan",
              style: TextStyle(color: ThemeColors.latte),
            ),
          ],
        ),
      ),
    );
  }
}
