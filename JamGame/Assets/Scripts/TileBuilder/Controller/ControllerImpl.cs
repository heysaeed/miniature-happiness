﻿using Common;
using Level;
using Level.Room;
using TileBuilder.Command;
using UnityEngine;
using UnityEngine.Events;

namespace TileBuilder.Controller
{
    public enum GameMode
    {
        God,
        Build,
        Play
    }

    [AddComponentMenu("Scripts/TileBuilder.Controller.ControllerImpl")]
    public partial class ControllerImpl : MonoBehaviour, IDragAndDropAgent
    {
        [SerializeField]
        private TileBuilderImpl tileBuilder;

        public UnityEvent BuiltValidatedOffice;

        public void ChangeGameMode(GameMode gameMode)
        {
            tileBuilder.ChangeGameMode(gameMode);
        }

        public void ValidateBuilding()
        {
            Result result = tileBuilder.ExecuteCommand(new ValidateBuilding());
            if (result.Success)
            {
                BuiltValidatedOffice?.Invoke();
            }
        }

        public void Hover(CoreModel coreModel)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                coreModel.TileUnionModel.PlacingProperties.ApplyRotation(
                    RotationDirection.Clockwise
                );
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Result<Vector2Int> matrixResult = tileBuilder.GridProperties.GetMatrixPosition(ray);
            if (matrixResult.Failure)
            {
                return;
            }

            coreModel.TileUnionModel.PlacingProperties.SetPosition(matrixResult.Data);
            ShowSelectedRoom command = new(coreModel);
            _ = tileBuilder.ExecuteCommand(command);
        }

        public Result Drop(CoreModel coreModel)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Result<Vector2Int> matrixResult = tileBuilder.GridProperties.GetMatrixPosition(ray);
            if (matrixResult.Success)
            {
                coreModel.TileUnionModel.PlacingProperties.SetPosition(matrixResult.Data);
                return tileBuilder.ExecuteCommand(new DropRoom(coreModel));
            }
            else
            {
                return new FailResult(matrixResult.Error);
            }
        }

        public Result<CoreModel> Borrow()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Result<Vector2Int> matrixResult = tileBuilder.GridProperties.GetMatrixPosition(ray);
            if (matrixResult.Success)
            {
                BorrowRoom command = new(matrixResult.Data);
                Result result = tileBuilder.ExecuteCommand(command);
                return result.Success
                    ? new SuccessResult<CoreModel>(command.BorrowedRoom)
                    : new FailResult<CoreModel>(result.Error);
            }
            return new FailResult<CoreModel>(matrixResult.Error);
        }

        public void HoverLeave()
        {
            _ = tileBuilder.ExecuteCommand(new HideSelectedRoom());
        }
    }
}