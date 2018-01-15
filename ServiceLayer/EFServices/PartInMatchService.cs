using ParsiBin.DataLayer;
using ParsiBin.DomainClasses.Entities;
using ParsiBin.ViewModel.AdminModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.EFServices
{
    public class PartInMatchService : IpartInMatch
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<ParticipantInMatch> _PartInMatch;
        private readonly IDbSet<Participant> _participant;   
        #endregion

        #region Constructor
        public PartInMatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _PartInMatch = _unitOfWork.Set<ParticipantInMatch>();
            _participant = _unitOfWork.Set<Participant>();            
        }
        #endregion

        #region List
        

        
        #endregion

       
        #region Add/Delete/Edit/View

        public void Add(PartInMatchModel viewModel)
        {            
            var lst = new ParticipantInMatch
            {   
                IsHomeTeam = viewModel.IsHomeTeam,
                Match = viewModel.Match,
                MatchID = viewModel.MatchID,
                Participant = viewModel.Participant,
                Participant_ID = viewModel.Participant_ID
            };
            _PartInMatch.Add(lst);            
        }

        public int GetMatchHomeParticipant(Guid MatchID)
        {
            return _PartInMatch.Where(x => x.MatchID == MatchID && x.IsHomeTeam == true).Select(x=>x.Participant_ID).FirstOrDefault();
        }

        public int GetMatchAwayParticipant(Guid MatchID)
        {
            return _PartInMatch.Where(x => x.MatchID == MatchID && x.IsHomeTeam == false).Select(x => x.Participant_ID).FirstOrDefault();
        }

        public void Delete(Guid MatchID)
        {
            var lst = _PartInMatch.Where(x => x.MatchID == MatchID).ToList();
            foreach (var item in lst)
            {
                _PartInMatch.Remove(item);
            }
            
        }


        #endregion
    }
}
