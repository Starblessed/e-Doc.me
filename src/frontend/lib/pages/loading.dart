import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:frontend/styles/colors.dart';

import 'package:frontend/pages/result.dart';
import 'package:frontend/services/scanner.dart';

class LoadingPage extends StatefulWidget {
  const LoadingPage({super.key});

  @override
  State<LoadingPage> createState() => _LoadingPageState();
}

class _LoadingPageState extends State<LoadingPage> {
  bool _isNavigating = false; // Evita múltiplas navegações

  @override
  void initState() {
    super.initState();
    // WidgetsBinding garante que o context e arguments já estejam prontos
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _startPolling();
    });
  }

  void _startPolling() async {
    final route = ModalRoute.of(context);
    final args = route?.settings.arguments;

    if (args is! ScanTaskResponse) {
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Argumentos inválidos')));
      Navigator.pop(context);
      return;
    }

    try {
      final result = await waitForResult(args.taskId);

      if (!mounted || _isNavigating) return;

      _isNavigating = true;
      print("${result.resultPath}");
      Navigator.pushReplacementNamed(
        context,
        '/result',
        arguments: ResultArguments(result.resultPath),
      );
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(e.toString())));

      await Future.delayed(const Duration(seconds: 2));
      if (mounted) Navigator.pop(context);
    }
  }

  Future<ScanTaskResult> waitForResult(String taskId) async {
    while (mounted) {
      // Verifica se a tela ainda existe
      final result = await fetchDocumentScanResult(taskId);

      if (result.status == "SUCCESS") return result;
      if (result.status == "FAILURE") throw Exception("Erro no servidor");

      await Future.delayed(const Duration(seconds: 1));
    }
    throw Exception("Operação cancelada");
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: ThemeColors.aquaForest,
      body: Center(
        // Center ajuda no alinhamento da coluna
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            SpinKitFadingCube(color: ThemeColors.gold, size: 50.0),
            const SizedBox(height: 150.0),
            const Text(
              "Your document is being processed...",
              style: TextStyle(
                fontSize: 15.0,
                fontWeight: FontWeight.w400,
                color: ThemeColors.offwhite,
              ),
            ),
          ],
        ),
      ),
    );
  }
}

Widget promoLoader = Row(
  mainAxisAlignment: MainAxisAlignment.center,
  children: [
    Text(
      "e",
      style: TextStyle(
        fontSize: 150.0,
        fontWeight: FontWeight.bold,
        color: ThemeColors.gold,
        fontStyle: FontStyle.italic,
      ),
    ),
    Column(
      children: [
        SizedBox(height: 100.0),
        SpinKitDoubleBounce(color: ThemeColors.gold, size: 50.0),
      ],
    ),
  ],
);
